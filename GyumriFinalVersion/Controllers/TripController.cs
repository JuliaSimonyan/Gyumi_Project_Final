using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gyumri.Common.Utility;
using Microsoft.EntityFrameworkCore;
using Gyumri.Common.ViewModel.Place;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
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
            return RedirectToAction("SecondStep");
        }

        //[HttpGet]
        //public async Task<IActionResult> SecondStep(PlaceType placeType = PlaceType.HOTEL, int currentPage = 1)
        //{
        //    //SetCulture();
        //    //var categories = await _categoryService.GetAllCategories();
        //    //var seeAndDoCategory = categories.FirstOrDefault(c => c.Name == "See & Do");

        //    //var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);
        //    //if (subcategoryId == -1 && subcategories.Any())
        //    //{
        //    //    subcategoryId = subcategories.First().SubcategoryId;
        //    //}

        //    //var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

        //    //ViewBag.PagesCount = (int)Math.Ceiling((double)places.Count() / 5);
        //    //ViewBag.AllSubcategories = subcategories;
        //    //ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
        //    //ViewBag.CurrentPage = currentPage;
        //    //ViewBag.Places = places.Skip((currentPage - 1) * 5).Take(5).ToList();
        //    //ViewBag.Categories = categories;

        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> SecondStep()
        {/*await _placeService.GetPlacesWithPlaceType()*/
            var allPlaces = new List<PlacesViewModel>
            {
                new PlacesViewModel
                {
                    Id = 1,
                    PlaceName = "Sunny Hotel",
                    PlaceNameArm = "Արևոտ Հյուրանոց",
                    PlaceNameRu = "Санни Отель",
                    Description = "A cozy hotel in downtown.",
                    DescriptionArm = "Հարմարավետ հյուրանոց քաղաքի կենտրոնում։",
                    DescriptionRu = "Уютный отель в центре города.",
                    Photo = "sunnyhotel.jpg",
                    MinPrice = 20000,
                    MaxPrice = 50000,
                    Raiting = 5,
                    ArticleId = null,
                    SubcategoryId = 1,
                    Address = "123 Main St",
                    AddressArm = "Հիմնական 123",
                    AddressRu = "Главная 123",
                    PlaceType = PlaceType.HOTEL
                },
                new PlacesViewModel
                {
                    Id = 2,
                    PlaceName = "Green Hostel",
                    PlaceNameArm = "Կանաչ Հոսթել",
                    PlaceNameRu = "Зелёный Хостел",
                    Description = "Affordable place for backpackers.",
                    DescriptionArm = "Մատչելի վայր ճանապարհորդների համար։",
                    DescriptionRu = "Доступное место для путешественников.",
                    Photo = "greenhostel.jpg",
                    MinPrice = 8000,
                    MaxPrice = 15000,
                    Raiting = 3,
                    ArticleId = 2,
                    SubcategoryId = 2,
                    Address = "456 Backpacker Ave",
                    AddressArm = "Ուղևորի 456",
                    AddressRu = "Бэкпекер 456",
                    PlaceType = PlaceType.HOSTEL
                },
                new PlacesViewModel
                {
                    Id = 3,
                    PlaceName = "Family Guesthouse",
                    PlaceNameArm = "Ընտանեկան Հյուրատուն",
                    PlaceNameRu = "Семейный Гостевой Дом",
                    Description = "Perfect for families.",
                    DescriptionArm = "Իդեալական ընտանեկան հանգստի համար։",
                    DescriptionRu = "Идеально для семейного отдыха.",
                    Photo = "familyguesthouse.jpg",
                    MinPrice = 12000,
                    MaxPrice = 30000,
                    Raiting = 4,
                    ArticleId = null,
                    SubcategoryId = 3,
                    Address = "789 Family Rd",
                    AddressArm = "Ընտանիքի 789",
                    AddressRu = "Семейная 789",
                    PlaceType = PlaceType.GUESTHOUSE
                }
            };


            ViewBag.Places = allPlaces;

            return View();
        }

        //[HttpPost]
        //public IActionResult SecondStep(int placeId)
        //{
        //    TempData["Place1Id"] = placeId;
        //    return RedirectToAction("ThirdStep");
        //}
        [HttpPost]
        public IActionResult ThirthStep(List<int> selectedPlaceIds)
        {
            if (selectedPlaceIds == null || selectedPlaceIds.Count == 0)
            {
                ModelState.AddModelError("", "Խնդրում ենք ընտրել առնվազն մեկ տեղ։");
                return RedirectToAction("SecondStep");
            }

            // Process the selected places here...

            return RedirectToAction("NextStep"); // or wherever you want to go
        }

        //[HttpGet]
        //public async Task<IActionResult> ThirdStep(int subcategoryId = -1, int currentPage = 1)
        //{
        //    SetCulture();
        //    var categories = await _categoryService.GetAllCategories();
        //    var relaxCategory = categories.FirstOrDefault(c => c.Name == "Relax & Sleep");

        //    var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(relaxCategory.CategoryId);
        //    if (subcategoryId == -1 && subcategories.Any())
        //    {
        //        subcategoryId = subcategories.First().SubcategoryId;
        //    }

        //    var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

        //    ViewBag.PagesCount = (int)Math.Ceiling((double)places.Count() / 5);
        //    ViewBag.AllSubcategories = subcategories;
        //    ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
        //    ViewBag.CurrentPage = currentPage;
        //    ViewBag.Places = places.Skip((currentPage - 1) * 5).Take(5).ToList();
        //    ViewBag.Categories = categories;

        //    return View();
        //}

        [HttpPost]
        public IActionResult ThirdStep(int placeId)
        {
            TempData["Place2Id"] = placeId;
            return RedirectToAction("ForthStep");
        }

        public async Task<IActionResult> ForthStep()
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            ViewBag.Categories = await _categoryService.GetAllCategories();

            if (!TempData.TryGetValue("Place1Id", out var place1IdObj) || !TempData.TryGetValue("Place2Id", out var place2IdObj))
            {
                return RedirectToAction("Index");
            }

            int place1Id = Convert.ToInt32(place1IdObj);
            int place2Id = Convert.ToInt32(place2IdObj);

            var place1 = await _placeService.GetPlaceById(place1Id);
            var place2 = await _placeService.GetPlaceById(place2Id);
            ViewBag.CurrentCulture = string.IsNullOrEmpty(currentCulture) ? "en" : currentCulture;
            ViewBag.Place1 = place1;
            ViewBag.Place2 = place2;

            return View();
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
