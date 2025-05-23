using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gyumri.Common.ViewModel.Place;
using GyumriFinalVersion.Models;
using System.Threading.Tasks;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private static TripInputInfo tripInfo= new();
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
            TempData["Place1Id"] = selectedPlaceId;
            tripInfo.PlaceWhereToStay = await _placeService.GetPlaceById(selectedPlaceId);
            return RedirectToAction("WhatToDo");
        }

        [HttpGet]
        public async Task<IActionResult> WhatToDo()
        {
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
            ViewBag.PlaceWhereToStay = tripInfo.PlaceWhereToStay;
            ViewBag.PlacesWhatToDo = tripInfo.PlaceWhatToDo;
            ViewBag.FullInfo = tripInfo;

            return View(tripInfo);
        }


        [HttpGet]
        public async Task<IActionResult> ForthStep()
        {

            ViewBag.PlaceWhereToStay = tripInfo.PlaceWhereToStay;
            ViewBag.PlacesWhatToDo = tripInfo.PlaceWhatToDo;
            ViewBag.FullInfo = tripInfo;

            return View(tripInfo);
        }

        public async Task<IActionResult> Final()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
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

        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmail(string email)
        {
            SetCulture();

            if (!TempData.TryGetValue("Place1Id", out var place1IdObj) || !TempData.TryGetValue("Place2Id", out var place2IdObj))
            {
                return RedirectToAction("Index");
            }

            int place1Id = Convert.ToInt32(place1IdObj);
            int place2Id = Convert.ToInt32(place2IdObj);

            var place1 = await _placeService.GetPlaceById(place1Id);
            var place2 = await _placeService.GetPlaceById(place2Id);

            var subject = AppRes.WelcomeGyumri;
            string body;

            string currentCulture = Request.Cookies["UserCulture"] ?? "en";

            if (currentCulture == "ru-RU")
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceNameRu ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceNameRu ?? "N/A")}</p>";
            }
            else if (currentCulture == "hy-AM")
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceNameArm ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceNameArm ?? "N/A")}</p>";
            }
            else
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceName ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceName ?? "N/A")}</p>";
            }


            SendEmail(email, subject, body);

            return RedirectToAction("Final");
        }
    }
}
