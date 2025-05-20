using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using DinkToPdf.Contracts;

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

        [HttpGet]
        public async Task<IActionResult> SecondStep(int subcategoryId = -1, int currentPage = 1)
        {
            SetCulture();
            var categories = await _categoryService.GetAllCategories();
            var seeAndDoCategory = categories.FirstOrDefault(c => c.Name == "See & Do");

            var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);
            if (subcategoryId == -1 && subcategories.Any())
            {
                subcategoryId = subcategories.First().SubcategoryId;
            }

            var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

            ViewBag.PagesCount = (int)Math.Ceiling((double)places.Count() / 5);
            ViewBag.AllSubcategories = subcategories;
            ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
            ViewBag.CurrentPage = currentPage;
            ViewBag.Places = places.Skip((currentPage - 1) * 5).Take(5).ToList();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public IActionResult SecondStep(int placeId)
        {
            TempData["Place1Id"] = placeId;
            return RedirectToAction("ThirdStep");
        }

        [HttpGet]
        public async Task<IActionResult> ThirdStep(int subcategoryId = -1, int currentPage = 1)
        {
            SetCulture();
            var categories = await _categoryService.GetAllCategories();
            var relaxCategory = categories.FirstOrDefault(c => c.Name == "Relax & Sleep");

            var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(relaxCategory.CategoryId);
            if (subcategoryId == -1 && subcategories.Any())
            {
                subcategoryId = subcategories.First().SubcategoryId;
            }

            var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

            ViewBag.PagesCount = (int)Math.Ceiling((double)places.Count() / 5);
            ViewBag.AllSubcategories = subcategories;
            ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
            ViewBag.CurrentPage = currentPage;
            ViewBag.Places = places.Skip((currentPage - 1) * 5).Take(5).ToList();
            ViewBag.Categories = categories;

            return View();
        }

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
            ViewBag.CurrentCulture = currentCulture;
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
