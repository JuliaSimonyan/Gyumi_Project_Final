using Gyumri.App.Interfaces;
using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Place;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class PlaceController : Controller
    {
        private readonly IPlace _placeService;
        private readonly ISubcategory _subcategoryService;
        private readonly IArticle _articleService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlaceController(IPlace placeService, ISubcategory subcategoryService, IArticle article, IWebHostEnvironment webHostEnvironment)
        {
            _placeService = placeService;
            _subcategoryService = subcategoryService;
            _articleService = article;
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
            var articles = await _articleService.GetAllArticles();
            ViewBag.Subcategories = subcategories;
            ViewBag.Articles = articles;
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
            var articles = await _articleService.GetAllArticles();
            ViewBag.Articles = articles;

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
                MinPrice = place.MinPrice,
                MaxPrice = place.MaxPrice,
                Raiting = place.Raiting,
                ArticleId = place.ArticleId,
                SubcategoryId = place.SubcategoryId,
                PlaceType = place.PlaceType,
                Address = place.Address,
                AddressArm = place.AddressArm,
                AddressRu = place.AddressRu,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditPlaceViewModel model, IFormFile photo)
        {
            var existingActivity = await _placeService.GetPlaceById(model.Id);

            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingActivity.Photo))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/places", existingActivity.Photo);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/places", fileName);
                model.Photo = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }
            else
            {
                model.Photo = existingActivity.Photo;
            }

            await _placeService.EditPlace(model);
            return RedirectToAction(nameof(Index));
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
