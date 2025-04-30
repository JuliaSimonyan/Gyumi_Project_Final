using Gyumri.App.Interfaces;
using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private const string CultureCookieName = "UserCulture";
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly IApartment _apartmentService;
        private readonly IActivityService _activityService;


        public TripController(ICategory categoryService, ApplicationContext context, IApartment apartment, IActivityService activityService)
        {
            _categoryService = categoryService;
            _context = context;
            _apartmentService = apartment;
            _activityService = activityService;
        }

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
        public async Task<IActionResult> FirstStep()
        {
           
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> SecondStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Apartments = await _apartmentService.GetAllApartments();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> ThirdStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Activities = await _activityService.GetAllActivities();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> ForthStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Apartments = await _apartmentService.GetAllApartments();
            ViewBag.Activities = await _activityService.GetAllActivities();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> Final(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Apartments = await _apartmentService.GetAllApartments();
            ViewBag.Activities = await _activityService.GetAllActivities();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }   
    }
}
