using Gyumri.App.Interfaces;
using Gyumri.App.Services;
using Gyumri.Application.Services;
using Gyumri.Common.ViewModel.Activity;
using Gyumri.Common.ViewModel.Apartment;
using Gyumri.Common.ViewModel.Place;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GyumriFinalVersion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ApartmentController : Controller
    {
        private readonly IApartment _apartmentService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ApartmentController(IApartment apartmentService, IWebHostEnvironment webHostEnvironment)
        {
            _apartmentService = apartmentService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var apartments = await _apartmentService.GetAllApartments();
            return View(apartments);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ApartmentViewModel model, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/accomodations", fileName);
                model.Image = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            await _apartmentService.AddApartment(model);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var apartment = await _apartmentService.GetApartmentById(id);
            if (apartment == null)
            {
                return NotFound();
            }
            return View(apartment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApartmentViewModel model, IFormFile photo)
        {
            var existingActivity = await _apartmentService.GetApartmentById(model.Id);

            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingActivity.Image))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/accomodations", existingActivity.Image);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/accomodations", fileName);
                model.Image = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }
            else
            {
                model.Image = existingActivity.Image;
            }

            await _apartmentService.EditApartment(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apartmentService.DeleteApartment(id);
            return RedirectToAction("Index");
        }

    }
}
