using Gyumri.Application.Interfaces;
using Gyumri.Application.Services;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GyumriFinalVersion.Controllers
{
    public class MapController : Controller
    {
        private const string CultureCookieName = "UserCulture";
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subcategoryService;
        private readonly IPlace _placeService;

        public MapController(ICategory categoryService, ApplicationContext context, ISubcategory subcategoryService, IPlace placeService)
        {
            _categoryService = categoryService;
            _context = context;
            _subcategoryService = subcategoryService;
            _placeService = placeService;
        }

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

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
            return View();
        }
    }
}
