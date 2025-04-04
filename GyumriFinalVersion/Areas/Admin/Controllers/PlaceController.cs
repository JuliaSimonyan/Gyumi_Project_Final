using Gyumri.Application.Interfaces;
using Gyumri.Application.Services;
using Gyumri.Common.ViewModel.Place;
using Gyumri.Common.ViewModel.Subcategory;
using Microsoft.AspNetCore.Mvc;

namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaceController : Controller
    {
        private readonly IPlace _placeService;
        private readonly ISubcategory _subcategoryService;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public PlaceController(IPlace placeService, ISubcategory subcategoryService, IWebHostEnvironment webHostEnvironment)
        {
            _placeService = placeService;
            _subcategoryService = subcategoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var places = await _placeService.GetAllPlaces();

            return View(places);
        }
        public async Task<IActionResult> Add()
        {
            var subcategories = await _subcategoryService.GetAllSubcategories();
            ViewBag.Subcategories = subcategories;

            return View();
        }
        [HttpPost]
        public IActionResult Add(AddEditPlaceViewModel model, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(photo.FileName);
                string path = $"{_webHostEnvironment.WebRootPath}/Images/places/{fileName}";
                model.Photo = fileName;
                using var fileStream = new FileStream(path, FileMode.Create);
                photo.CopyTo(fileStream);
            }
            bool place = _placeService.AddPlace(model);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool isPlaceDeleted = _placeService.DeletePlace(id);
            if (isPlaceDeleted)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }



    }
}
