using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using DinkToPdf.Contracts;
using DinkToPdf;
using Gyumri.Application.Services;
using System.Drawing.Printing;
using Gyumri.Common.ViewModel.Place;
using Gyumri.Common.ViewModel.Subcategory;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private const string CultureCookieName = "UserCulture";
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subCategoryService;
        private readonly IPlace _placeService;

        private readonly IConverter _converter;

        public TripController(ICategory categoryService, ApplicationContext context, IPlace placeServoce, ISubcategory subcategoryService)
        {
            _categoryService = categoryService;
            _context = context;
            _subCategoryService = subcategoryService;
            _placeService = placeServoce;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string dates, string adult, string children)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            if (dates is null && adult is null && children is null)
            {
                return View();
            }
            TempData["Dates"] = dates;
            TempData["AductCount"] = adult;
            TempData["ChildrenCount"] = children;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return RedirectToAction("FirstStep");
        }

        [HttpGet]
        public async Task<IActionResult> FirstStep()
        {

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FirstStep(string transport)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            TempData[transport] = transport;

            return RedirectToAction("SecondStep");
        }

        [HttpGet]
        public async Task<IActionResult> SecondStep(int subcategoryId = -1, int currentPage = 1)
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var categories = await _categoryService.GetAllCategories();
            var seeAndDoCategory = categories.FirstOrDefault(c => c.Name == "See & Do");
            var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);

            // Create test subcategories if none exist
            if (!subcategories.Any())
            {
                for (int i = 1; i <= 2; i++)
                {
                    var newSubcategory = new AddSubcategoryViewModel
                    {
                        Name = $"Test Subcategory {i}",
                        NameArm = $"Թեստ Սուբկատեգորիա {i}",
                        NameRu = $"Тестовая Подкатегория {i}",
                        Description = $"Test Subcategory {i}",
                        DescriptionArm = $"Թեստ Սուբկատեգորիա {i}",
                        DescriptionRu = $"Тестовая Подкатегория {i}",
                        CategoryId = seeAndDoCategory.CategoryId
                    };
                    await _subCategoryService.AddSubcategory(newSubcategory);
                }
                subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);
            }

            foreach (var sub in subcategories)
            {
                var existingPlaces = await _placeService.GetPlacesBySubCategoryId(sub.SubcategoryId);

                for (int i = 1; i <= 25; i++)
                {
                    var newPlace = new AddEditPlaceViewModel
                    {
                        SubcategoryId = sub.SubcategoryId,
                        PlaceName = $"Test Place {i} - {sub.Name}",
                        PlaceNameArm = $"Թեստ Տեղ {i} - {sub.NameArm}",
                        PlaceNameRu = $"Тестовое место {i} - {sub.NameRu}",
                        Description = "This is a test description.",
                        DescriptionArm = "Սա թեստ նկարագրություն է։",
                        DescriptionRu = "Это тестовое описание.",
                        MinPrice = 10000 + i * 1000,
                        MaxPrice = 15000 + i * 1000,
                        Photo = "default.jpg"
                    };
                    await _placeService.AddPlace(newPlace);
                }
            }

            if (subcategoryId == -1 && subcategories.Any())
            {
                subcategoryId = subcategories.First().SubcategoryId;
            }

            var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

            ViewBag.PagesCount = places.Any() ? (int)Math.Ceiling((double)places.Count() / 5) : 1;
            ViewBag.AllSubcategories = subcategories;
            ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
            ViewBag.CurrentPage = currentPage;

            ViewBag.Places = places
                                .Skip((currentPage - 1) * 5)
                                .Take(5)
                                .ToList();

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
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var categories = await _categoryService.GetAllCategories();
            var seeAndDoCategory = categories.FirstOrDefault(c => c.Name == "Relax & Sleep");
            var subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);

            // Create test subcategories if none exist
            if (!subcategories.Any())
            {
                for (int i = 1; i <= 2; i++)
                {
                    var newSubcategory = new AddSubcategoryViewModel
                    {
                        Name = $"Test Subcategory Relax & Sleep {i}",
                        NameArm = $"Թեստ Սուբկատեգորիա {i}",
                        NameRu = $"Тестовая Подкатегория {i}",
                        Description = $"Test Subcategory {i}",
                        DescriptionArm = $"Թեստ Սուբկատեգորիա {i}",
                        DescriptionRu = $"Тестовая Подкатегория {i}",
                        CategoryId = seeAndDoCategory.CategoryId
                    };
                    await _subCategoryService.AddSubcategory(newSubcategory);
                }
                subcategories = await _subCategoryService.GetSubcategoriesByCategoryId(seeAndDoCategory.CategoryId);
            }

            foreach (var sub in subcategories)
            {
                var existingPlaces = await _placeService.GetPlacesBySubCategoryId(sub.SubcategoryId);

                for (int i = 1; i <= 25; i++)
                {
                    var newPlace = new AddEditPlaceViewModel
                    {
                        SubcategoryId = sub.SubcategoryId,
                        PlaceName = $"Test Place Relax & Sleep {i} - {sub.Name}",
                        PlaceNameArm = $"Թեստ Տեղ {i} - {sub.NameArm}",
                        PlaceNameRu = $"Тестовое место {i} - {sub.NameRu}",
                        Description = "This is a test description.",
                        DescriptionArm = "Սա թեստ նկարագրություն է։",
                        DescriptionRu = "Это тестовое описание.",
                        MinPrice = 10000 + i * 1000,
                        MaxPrice = 15000 + i * 1000,
                        Photo = "default.jpg"
                    };
                    await _placeService.AddPlace(newPlace);
                }
            }

            if (subcategoryId == -1 && subcategories.Any())
            {
                subcategoryId = subcategories.First().SubcategoryId;
            }

            var places = await _placeService.GetPlacesBySubCategoryId(subcategoryId);

            ViewBag.PagesCount = places.Any() ? (int)Math.Ceiling((double)places.Count() / 5) : 1;
            ViewBag.AllSubcategories = subcategories;
            ViewBag.CurrentSubcategory = subcategories.FirstOrDefault(sc => sc.SubcategoryId == subcategoryId);
            ViewBag.CurrentPage = currentPage;

            ViewBag.Places = places
                                .Skip((currentPage - 1) * 5)
                                .Take(5)
                                .ToList();

            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public IActionResult ThirdStep(int placeId)
        {
            TempData["Place2Id"] = placeId;
            return RedirectToAction("ForthStep");
        }

        public async Task<IActionResult> ForthStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var place1 = await _placeService.GetPlaceById(Convert.ToInt32((TempData["place1Id"])));
            var place2 = await _placeService.GetPlaceById(Convert.ToInt32((TempData["place2Id"])));

            ViewBag.Place1 = place1;
            ViewBag.Place2 = place2;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> Final(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();

            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        // ______________________Send email ____________________
        private readonly string fromEmail = "inesamkrtchyan9999@gmail.com";
        private readonly string password = "lhwd zttk xque sajp";

        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(fromEmail, "Your Name");
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmail(string email)
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var place1 = await _placeService.GetPlaceById(Convert.ToInt32((TempData["place1Id"])));
            var place2 = await _placeService.GetPlaceById(Convert.ToInt32((TempData["place2Id"])));

            var subject = AppRes.WelcomeGyumri;
            string body;

            if (currentCulture == "ru-RU")
            {
                body = $"<h1>{AppRes.TripPlan}</h1>" +
                $"<h3>{@AppRes.WheretoStay} </h3>" +
                $"<p>{place1.PlaceNameRu}</p>" +
                $"<p> place1.Address </p>" +
                $"<br/>" +
                $"<h3>{@AppRes.WhatToDo} </h3>" +
                $"<p>{place2.PlaceNameRu}</p>" +
                $"<p> place1.Address </p>";
            }
            else if (currentCulture == "hy-AM")
            {
                body = $"<h1>{AppRes.TripPlan}</h1>" +
                $"<h3>{@AppRes.WheretoStay} </h3>" +
                $"<p>{place1.PlaceNameArm}</p>" +
                $"<p> place1.Address </p>" +
                $"<br/>" +
                $"<h3>{@AppRes.WhatToDo} </h3>" +
                $"<p>{place2.PlaceNameArm}</p>" +
                $"<p> place1.Address </p>";
            }
            else
            {
                body = $"<h1>{AppRes.TripPlan}</h1>" +
                $"<h3>{@AppRes.WheretoStay} </h3>" +
                $"<p>{place1.PlaceName}</p>" +
                $"<p> place1.Address </p>" +
                $"<br/>" +
                $"<h3>{@AppRes.WhatToDo} </h3>" +
                $"<p>{place2.PlaceName}</p>" +
                $"<p> place1.Address </p>";
            }
            SendEmail(email, subject, body);

            return RedirectToAction("Index", "Home");
        }

        // ______________________Send email ____________________

        //________________Download PDF____________________

        //public IActionResult DownloadTripPlanPdf()
        //{
        //    var body = $"<h1>Ճանապարհորդության պլան</h1>" +
        //               $"<h3>Այստեղ կարող է լինել ձեր գովազդը :D </h3>";

        //    var doc = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = new GlobalSettings
        //        {
        //            PaperSize = PaperKind.A4,
        //            Orientation = Orientation.Portrait,
        //        },
        //        Objects = {
        //        new ObjectSettings
        //        {
        //            HtmlContent = body,
        //            WebSettings = { DefaultEncoding = "utf-8" }
        //        }
        //    }
        //    };

        //    var pdf = _converter.Convert(doc);

        //    return File(pdf, "application/pdf", "TripPlan.pdf");
        //}

        //________________Download PDF____________________
    }
}
