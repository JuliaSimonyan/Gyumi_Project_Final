using Gyumri.App.Interfaces;
using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GyumriFinalVersion.Controllers
{
    public class HomeController : Controller
    {
        private const string CultureCookieName = "UserCulture";
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subcategoryService;
        private readonly IPlace _placeService;
        private readonly IArticle _articleService;

        public HomeController(ICategory categoryService, ApplicationContext context, ISubcategory subcategoryService, IPlace placeService , IArticle articleService)
        {
            _categoryService = categoryService;
            _context = context;
            _subcategoryService = subcategoryService;
            _placeService = placeService;
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Subcategories = await _subcategoryService.GetAllSubcategories();
            ViewBag.Places = await _placeService.GetAllPlaces();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);

            // Set the culture for the request
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Category(int categoryId)
        {
            CategoryViewModel category = await _categoryService.GetCategoryById(categoryId);
            var subcategories = await _subcategoryService.GetSubcategoriesByCategoryId(categoryId);
            foreach (var item in subcategories)
            {
                item.Places = await _placeService.GetPlacesBySubCategoryId(item.SubcategoryId);
            }
            ViewBag.Subcategories = subcategories;
            ViewBag.Category = category;
            ViewBag.Categories = await _categoryService.GetAllCategories();

            if (categoryId > 0)
            {
                var selectedCategory = await _categoryService.GetCategoryById(categoryId);
                ViewBag.CategoryName = selectedCategory?.Name;
            }
            return View();
        }

        [HttpGet]
        [Route("Home/Articles/{articleId}")]

        public async Task<IActionResult> Articles(int articleId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);

            var place = await _placeService.GetPlaceByArticleId(articleId);
            if (place == null)
            {
                return NotFound();
            }

            ViewBag.PlaceImage = place.Photo;
            ViewBag.PlaceSubcategoryId = place.SubcategoryId;

            var subcategory = await _subcategoryService.GetSubcategoryById(place.SubcategoryId);
            ViewBag.SubcategoryName = subcategory?.Name; 

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var article = await _articleService.GetArticleById(articleId);
            if (article == null)
                return NotFound();

            ViewBag.Articles = await _articleService.GetAllArticles();
            return View(article);
        }

        [HttpPost] // Make this a POST request
        public async Task<IActionResult> SavePlace(int articleId)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.ArticleId == articleId);
            if (place == null)
            {
                return NotFound(); // No place linked to this article
            }

            var saved = HttpContext.Session.Get<List<int>>("SavedPlaces") ?? new List<int>();

            if (!saved.Contains(place.PlaceId))
            {
                saved.Add(place.PlaceId);
                HttpContext.Session.Set("SavedPlaces", saved);
            }

            return RedirectToAction("MyList");
        }

        public async Task<IActionResult> MyList()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();

            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;


            var savedIds = HttpContext.Session.Get<List<int>>("SavedPlaces") ?? new List<int>();

            var places = await _context.Places
                .Where(p => savedIds.Contains(p.PlaceId))
                .ToListAsync();


            return View(places);
        }
        public class RemovePlaceRequest
        {
            public int PlaceId { get; set; }
        }

        [HttpPost]
        public IActionResult RemovePlace([FromBody] RemovePlaceRequest request)
        {
            var placeId = request.PlaceId;

            var savedPlaces = HttpContext.Session.Get<List<int>>("SavedPlaces") ?? new List<int>();
            if (savedPlaces.Contains(placeId))
            {
                savedPlaces.Remove(placeId);
                HttpContext.Session.Set("SavedPlaces", savedPlaces);
            }

            return Ok();
        }


        public async Task<IActionResult> Test()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Subcategories = await _subcategoryService.GetAllSubcategories();
            ViewBag.Places = await _placeService.GetAllPlaces();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);

            // Set the culture for the request
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            return View();
        }

        public IActionResult ChangeLanguage(string lang)
        {

            // Պահել լեզուն cookie-ում
            Response.Cookies.Append(CultureCookieName, lang, new Microsoft.AspNetCore.Http.CookieOptions
            {

                Expires = DateTime.UtcNow.AddYears(1),  // Լեզուն պահպանվում է 1 տարի
                IsEssential = true, // Անհրաժեշտ է կարգավորումների համար
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax // Համակարգչի միջավայրում
            });

            // Արտահանենք լեզվով փոխված էջը
            return RedirectToAction("Index");
        }


    


    }
}
