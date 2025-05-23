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

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private static TripInputInfo tripInfo = new();
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subCategoryService;
        private readonly IPlace _placeService;

        private readonly string fromEmail = "inesamkrtchyan9999@gmail.com";
        private readonly string password = "lhwd zttk xque sajp";

        public TripController(ICategory categoryService, ApplicationContext context, IPlace placeService, ISubcategory subcategoryService)
        {
            _categoryService = categoryService;
            _context = context;
            _subCategoryService = subcategoryService;
            _placeService = placeService;
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
                        // ավելացրու այլ հատկությունները եթե կան
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


        [HttpGet]
        public async Task<IActionResult> ForthStep()
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

        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmail(string email)
        {
            SetCulture();

            var subject = AppRes.WelcomeGyumri;
            var body = new StringBuilder();

            body.AppendLine("<h1>Trip to Gyumri</h1>");
            body.AppendLine("<p>Plan</p>");
            body.AppendLine("<br/>");
            body.AppendLine($"<p>Date - {tripInfo.Date}</p>");
            body.AppendLine($"<p>Adult count - {tripInfo.AdultCount}</p>");
            body.AppendLine($"<p>Children count - {tripInfo.ChildrenCount}</p>");
            body.AppendLine("<br/>");

            body.AppendLine("<h2>Where to stay</h2>");
            body.AppendLine($"<p>Place Name {tripInfo.PlaceWhereToStay?.PlaceName}</p>");
            body.AppendLine($"<p>Address {tripInfo.PlaceWhereToStay?.Address}</p>");
            body.AppendLine("<br/>");

            body.AppendLine("<h2>What to do</h2>");
            foreach (var trip in tripInfo.PlaceWhatToDo)
            {
                body.AppendLine($"<p>Place name - {trip.PlaceName}</p>");
                body.AppendLine($"<p>Address - {trip.Address}</p>");
                body.AppendLine("<br/>");
            }

            SendEmail(email, subject, body.ToString());

            return RedirectToAction("Final");
        }
    }
}
