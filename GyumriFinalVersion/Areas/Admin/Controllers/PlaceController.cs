using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Place;
using Gyumri.Common.ViewModel.Subcategory;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Add(AddEditPlaceViewModel model, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/places", fileName);
                model.Photo = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            bool success = await _placeService.AddPlace(model);
            if (!success)
            {
                ModelState.AddModelError("", "Error adding place.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var place = await _placeService.GetPlaceById(id);
            if (place == null)
            {
                return NotFound();
            }

            var subcategories = await _subcategoryService.GetAllSubcategories();
            ViewBag.Subcategories = subcategories;

            var model = new AddEditPlaceViewModel
            {
                Id = place.Id,
                PlaceName = place.PlaceName,
                PlaceNameArm = place.PlaceNameArm,
                PlaceNameRu = place.PlaceNameRu,
                Description = place.Description ?? "",
                DescriptionArm = place.DescriptionArm,
                DescriptionRu = place.DescriptionRu,
                Photo = place.Photo,
                SubcategoryId = place.SubcategoryId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditPlaceViewModel model, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/places", fileName);
                model.Photo = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            bool success = await _placeService.EditPlace(model);
            if (!success)
            {
                ModelState.AddModelError("", "Error updating place.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isPlaceDeleted = await _placeService.DeletePlace(id);
            if (isPlaceDeleted)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
