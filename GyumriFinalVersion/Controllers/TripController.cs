using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gyumri.Common.ViewModel.Place;
using GyumriFinalVersion.Models;
using GyumriFinalVersion.Services;
using Gyumri.Common.Utility;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private static TripInputInfo tripInfo = new();
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subCategoryService;
        private readonly IPlace _placeService;
        private readonly PdfGenerator _pdfGenerator;
        private readonly IWebHostEnvironment _env;
        private readonly string fromEmail = "visitgyumri.info@gmail.com";
        private readonly string password = "avmy aviv ukql syui";

        public TripController(IWebHostEnvironment env, PdfGenerator PdfGenerator, ICategory categoryService, ApplicationContext context, IPlace placeService, ISubcategory subcategoryService)
        {
            _categoryService = categoryService;
            _context = context;
            _subCategoryService = subcategoryService;
            _placeService = placeService;
            _pdfGenerator = PdfGenerator;
            _env = env;
        }

        private void SetCulture()
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            tripInfo = new();
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string dates, string adult, string children)
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();

            if (string.IsNullOrEmpty(dates) && string.IsNullOrEmpty(adult) && string.IsNullOrEmpty(children))
            {
                return View();
            }
            tripInfo.Date = dates;
            tripInfo.AdultCount = adult;
            tripInfo.ChildrenCount = children;
            TempData["Dates"] = dates;
            TempData["AdultCount"] = adult;
            TempData["ChildrenCount"] = children;

            return RedirectToAction("FirstStep");
        }

        [HttpGet]
        public async Task<IActionResult> FirstStep()
        {
            SetCulture();

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public IActionResult FirstStep(string transport)
        {
            TempData["Transport"] = transport;
            tripInfo.TransportName = transport;
            tripInfo.TimePeriod = "90";
            tripInfo.TransportAmount = "12.000";
            return RedirectToAction("WheretoStay");
        }

        [HttpGet]
        public async Task<IActionResult> WheretoStay(PlaceType selectedPlaceType = PlaceType.HOTEL, int currentPage = 1)
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var Places = await _placeService.GetPlacesByPlaceType(selectedPlaceType);

            Console.WriteLine($"Selected Place Type: {selectedPlaceType}");

            ViewBag.PagesCount = (int)Math.Ceiling((double)Places.Count() / 5);
            ViewBag.CurrentPlaceType = selectedPlaceType;
            ViewBag.Places = Places.Skip((currentPage - 1) * 5).Take(5).ToList();
            ViewBag.CurrentPage = currentPage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WheretoStay(int selectedPlaceId)
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            TempData["Place1Id"] = selectedPlaceId;
            tripInfo.PlaceWhereToStay = await _placeService.GetPlaceById(selectedPlaceId);
            return RedirectToAction("WhatToDo");
        }

        [HttpGet]
        public async Task<IActionResult> WhatToDo()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var allPlaces = await _placeService.GetPlacesWithPlaceType();
            ViewBag.Places = allPlaces;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WhatToDo(List<int> selectedPlaceIds)
        {
            selectedPlaceIds = selectedPlaceIds.Distinct().ToList();

            if (tripInfo.PlaceWhatToDo == null)
            {
                tripInfo.PlaceWhatToDo = new List<PlacesViewModel>();
            }

            foreach (int id in selectedPlaceIds)
            {
                var placeEntity = await _placeService.GetPlaceById(id);
                if (placeEntity != null)
                {
                    // Պահանջվում է վերափոխում Place -> PlacesViewModel
                    var placeVm = new PlacesViewModel
                    {
                        Id = placeEntity.Id,
                        PlaceName = placeEntity.PlaceName,
                        PlaceNameArm = placeEntity.PlaceNameArm,
                        PlaceNameRu = placeEntity.PlaceNameRu,
                        Description = placeEntity.Description,
                        DescriptionArm = placeEntity.DescriptionArm,
                        DescriptionRu = placeEntity.DescriptionRu,
                        Photo = placeEntity.Photo,
                        MinPrice = placeEntity.MinPrice,
                        MaxPrice = placeEntity.MaxPrice,
                        Raiting = placeEntity.Raiting,
                        Address = placeEntity.Address,
                        AddressArm = placeEntity.AddressArm,
                        AddressRu = placeEntity.AddressRu,
                    };
                    tripInfo.PlaceWhatToDo.Add(placeVm);
                }
            }
            return RedirectToAction("LastStep"); // or wherever you want to go

        }

        [HttpGet]
        public async Task<IActionResult> LastStep()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.PlaceWhereToStay = tripInfo.PlaceWhereToStay;
            ViewBag.PlacesWhatToDo = tripInfo.PlaceWhatToDo;
            ViewBag.FullInfo = tripInfo;

            return View(tripInfo);
        }


        public async Task<IActionResult> Final()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.PlaceWhereToStay = tripInfo.PlaceWhereToStay;
            ViewBag.PlacesWhatToDo = tripInfo.PlaceWhatToDo;
            ViewBag.FullInfo = tripInfo;

            return View(tripInfo);
        }

        // ______________________ Send Email ____________________
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(fromEmail, "Gyumri Trip Planner");
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        public IActionResult Animation()
        {
            return View();
        }
        // Move logic to a shared method
        private byte[] GenerateTripPdf()
        {
            string htmlContent = Util.GetPdfHtml(tripInfo, _env);

            var pdfBytes = _pdfGenerator.GeneratePdf(htmlContent);
            return pdfBytes;
        }

        public void SendEmailWithAttachment(string toEmail, string subject, string htmlBody, byte[] attachmentBytes, string fileName)
        {
            var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            if (attachmentBytes != null)
            {
                message.Attachments.Add(new Attachment(new MemoryStream(attachmentBytes), fileName, "application/pdf"));
            }

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmail(string email)
        {
            // Configure SMTP settings directly (for quick use; not recommended for production)
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUser = fromEmail;
            var smtpPass = password;
            var enableSsl = true;

            // Build email body
            var subject = "Trip to Gyumri";
            string body = "<p>Please find your trip plan attached as a PDF.</p>";
         
         
            // Generate PDF in memory
            byte[] pdfBytes = GenerateTripPdf();

            // Prepare the email
            var message = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(email);

            // Attach the generated PDF
            var attachment = new Attachment(new MemoryStream(pdfBytes), "TripPlan.pdf", "application/pdf");
            message.Attachments.Add(attachment);

            // Send the email
            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = enableSsl;

                await smtp.SendMailAsync(message);
            }

            return RedirectToAction("Final");
        }

        //public IActionResult DownloadPdf()
        //{
        //    string htmlContent = @"";
        //    var pdfBytes = _pdfGenerator.GeneratePdf(htmlContent);
        //    return File(pdfBytes, "application/pdf", "downloaded.pdf");
        //}

        public IActionResult DownloadPdf()
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            var pdfBytes = GenerateTripPdf();

            return File(pdfBytes, "application/pdf", "downloaded.pdf");
        }

    }
}
