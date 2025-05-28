using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gyumri.Common.ViewModel.Place;
using GyumriFinalVersion.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using DinkToPdf.Contracts;
using GyumriFinalVersion.Services;
using iTextSharp.text.pdf;
using iTextSharp.text;

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
            string placeName = tripInfo?.PlaceWhereToStay?.PlaceName;
            //currentCulture == "ru-RU" ? fullInfo.PlaceWhereToStay?.PlaceNameRu :
            //               currentCulture == "hy-AM" ? fullInfo.PlaceWhereToStay?.PlaceNameArm :
            //               fullInfo.PlaceWhereToStay?.PlaceName;

            string placeAddress = tripInfo?.PlaceWhereToStay?.Address;
            //currentCulture == "ru-RU" ? fullInfo.PlaceWhereToStay?.AddressRu :
            //                  currentCulture == "hy-AM" ? fullInfo.PlaceWhereToStay?.AddressArm :
            //                  fullInfo.PlaceWhereToStay?.Address;
            string logoPath = Path.Combine(_env.WebRootPath, "content", "Images", "pdfIcon.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(logoPath);
            string base64Image = Convert.ToBase64String(imageBytes);
            string logoDataUri = $"data:image/png;base64,{base64Image}";

            string logoPath2 = Path.Combine(_env.WebRootPath, "content", "Images", "car.png");
            byte[] imageBytes2 = System.IO.File.ReadAllBytes(logoPath2);
            string base64Image2 = Convert.ToBase64String(imageBytes2);
            string carDataUri = $"data:image/png;base64,{base64Image2}";

            string logoPath3 = Path.Combine(_env.WebRootPath, "content", "Images", "hotel.png");
            byte[] imageBytes3 = System.IO.File.ReadAllBytes(logoPath3);
            string base64Image3 = Convert.ToBase64String(imageBytes3);
            string hotelUri = $"data:image/png;base64,{base64Image3}";

            string logoPath4 = Path.Combine(_env.WebRootPath, "content", "Images", "whatToDo.png");
            byte[] imageBytes4 = System.IO.File.ReadAllBytes(logoPath4);
            string base64Image4 = Convert.ToBase64String(imageBytes4);
            string whatToDoUri = $"data:image/png;base64,{base64Image4}";


            string css = @"
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: 'Montserrat', sans-serif;
}
.pdfLogo{
filter:brightness(0);
}
body {
    color: #333;
    line-height: 1.6;
    padding-top: 76px; /* Account for fixed navbar */
    min-height: 100vh;
    display: flex;
    flex-direction: column; /* This ensures proper stacking of elements */
}

.plan-container {
    margin:0 auto;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    width: 100%;
    max-width: 850px;
    padding: 30px;
    margin-bottom: 30px;
}

.plan-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 30px;
}

.logo {
    width: 40px;
    height: 40px;
}

.address {
    text-align: right;
    font-size: 12px;
    color: #666;
}

.plan-title {
    text-align: center;
    margin-bottom: 20px;
}

.plan-title h1 {
    font-size: 24px;
    font-weight: 600;
    margin-bottom: 5px;
}

.plan-title p {
    font-size: 16px;
    color: #666;
}

.plan-details {
    display: flex;
    justify-content: center;
    gap: 20px;
    margin-bottom: 30px;
    padding-bottom: 20px;
    border-bottom: 1px solid #eee;
}

.detail-item {
    display: flex;
    align-items: center;
    font-size: 14px;
}

.detail-item i {
    margin-right: 8px;
    color: #333;
}

.plan-section {
    margin-bottom: 25px;
}

.plan-section h2 {
    font-size: 18px;
    font-weight: 600;
    margin-bottom: 15px;
    color: #000;
}

.section-item {
    display: flex;
    align-items: center;
    margin-bottom: 15px;
    padding-bottom: 15px;
    border-bottom: 1px solid #f5f5f5;
}

.section-item:last-child {
    margin-bottom: 0;
    padding-bottom: 0;
    border-bottom: none;
}

.item-icon {
    width: 40px;
    height: 40px;
    background-color: #f5f5f5;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-right: 15px;
}

.item-name {
    flex: 1;
    font-weight: 500;
}

.item-info {
    display: flex;
    align-items: center;
    font-size: 14px;
    color: #666;
}

.item-info i {
    margin-left: 8px;
}

.action-buttons {
    display: flex;
    gap: 10px;
    flex-wrap: wrap;
    justify-content: center;
    margin-top: 20px;
    margin-bottom: 40px;
}

.btn {
    display: flex;
    align-items: center;
    padding: 10px 15px;
    border-radius: 30px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.2s;
    border: none;
}

.btn i {
    margin-right: 8px;
}

.btn-back {
    background-color: #f5f5f5;
    color: #333;
    border: 1px solid #ddd;
}

.btn-download {
    background-color: #1a1a1a;
    color: white;
}

.btn-email {
    background-color: #1a1a1a;
    color: white;
}

.btn-share {
    background-color: #1a1a1a;
    color: white;
}

.btn:hover {
    opacity: 0.9;
    transform: translateY(-2px);
}

/* Modal Styles */
.modal {
    display: none;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 1000;
    justify-content: center;
    align-items: center;
}

.modal-content {
    background-color: white;
    padding: 30px;
    border-radius: 8px;
    width: 90%;
    max-width: 500px;
    position: relative;
}

.close-modal {
    position: absolute;
    top: 15px;
    right: 15px;
    font-size: 24px;
    cursor: pointer;
    color: #666;
}

.modal h3 {
    margin-bottom: 20px;
    text-align: center;
}

.social-icons {
    display: flex;
    justify-content: center;
    gap: 15px;
    margin-bottom: 20px;
}

.social-icon {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    color: white;
    text-decoration: none;
    font-size: 18px;
}

.facebook {
    background-color: #3b5998;
}

.twitter {
    background-color: #1da1f2;
}

.whatsapp {
    background-color: #25d366;
}

.telegram {
    background-color: #0088cc;
}

.share-link {
    display: flex;
    margin-top: 20px;
}

.share-link input {
    flex: 1;
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 4px 0 0 4px;
    font-size: 14px;
}

.share-link button {
    padding: 10px 15px;
    background-color: #1a1a1a;
    color: white;
    border: none;
    border-radius: 0 4px 4px 0;
    cursor: pointer;
}
";

            string htmlContent = $@"
        <div class=""plan-container"">
            <div class=""plan-header"">
                <div class=""logo"">
                    <img class=""pdfLogo"" src=""{logoDataUri}"" alt=""Logo"" height=""30"">
                </div>
                <div class=""address"">
                    <p>{AppRes.AddressGyumri}</p>
                    <p>{AppRes.AddressGyumri2}</p>
                </div>
            </div>
            <div class=""plan-title"">
                <h1>{AppRes.TripToGyumri}</h1>
                <p>{AppRes.Plan}</p>
            </div>
            <div class=""plan-details"">
                <div class=""detail-item""><span>{tripInfo.Date}</span></div>
                <div class=""detail-item""><span>{AppRes.Adults} - {tripInfo.AdultCount}</span></div>
                <div class=""detail-item""><span> {AppRes.Children} - {tripInfo.ChildrenCount}</span></div>
            </div>
            <div class=""plan-section"">
                <h2>{AppRes.GettingtoGyumri}</h2>
                <div class=""section-item"">
                    <div class=""item-icon"">
                        <img class=""pdfLogo"" src=""{carDataUri}"" alt=""Logo"" height=""30"">
                    </div>
                    <div class=""item-name"">{tripInfo.TransportName}</div>
                    <div class=""item-info""><span>{tripInfo.TimePeriod} {AppRes.min}</span></div>
                </div>
            </div>
            <div class=""plan-section"">
                <h2>{AppRes.WheretoStay}</h2>
                <div class=""section-item"">
                    <div class=""item-icon"">
                        <img class=""pdfLogo"" src=""{hotelUri}"" alt=""Logo"" height=""30"">
                    </div>
                    <div class=""item-name"">{placeName}</div>
                    <div class=""item-info"">{placeAddress}</div>
                </div>
            </div>
        ";

            htmlContent += $@" <div class=""plan-section"">
                <h2>{AppRes.WhatToDo}</h2>";

            foreach (var item in tripInfo.PlaceWhatToDo)
            {
                htmlContent += $@"
                <div class=""section-item"">
                    <div class=""item-icon"">
                         <img class=""pdfLogo"" src=""{whatToDoUri}"" alt=""Logo"" height=""30"">
                    </div>
                <div class=""item-name"">{item?.PlaceName}</div>
                    <div class=""item-info"">{item?.Address}</div>
                </div>";
            }
            htmlContent += $@" </div>
                            </div>
                            <style>{css}</style>";
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
