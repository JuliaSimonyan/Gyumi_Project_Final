using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using GyumriFinalVersion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
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

        public HomeController(ICategory categoryService, ApplicationContext context, ISubcategory subcategoryService, IPlace placeService)
        {
            _categoryService = categoryService;
            _context = context;
            _subcategoryService = subcategoryService;
            _placeService = placeService;
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
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
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
            return View();
        }

        [HttpPost]
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
