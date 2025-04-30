using Gyumri.App.Interfaces;
using Gyumri.Common.ViewModel.Activity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gyumri.App.Services;
using Microsoft.EntityFrameworkCore;


namespace GyumriFinalVersion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ActivityController(IActivityService activityService, IWebHostEnvironment webHostEnvironment)
        {
            _activityService = activityService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task< IActionResult> Index()
        {
            var activities = await _activityService.GetAllActivities();
            return View(activities);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ActivityViewModel activityVm, IFormFile photo)
        {
            if (photo != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/activities", fileName);
                activityVm.Image = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            await _activityService.AddActivity(activityVm);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _activityService.GetActivityById(id);
            if (activity == null) return NotFound();

            return View(activity);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ActivityViewModel activityVm, IFormFile photo)
        {
            // Get the existing activity to preserve its image if no new image is uploaded
            var existingActivity = await _activityService.GetActivityById(activityVm.Id);

            if (photo != null && photo.Length > 0)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingActivity.Image))
                {
                    string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/activities", existingActivity.Image);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Upload new image
                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/activities", fileName);
                activityVm.Image = fileName;

                using var fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }
            else
            {
                // Keep the existing image if no new image was uploaded
                activityVm.Image = existingActivity.Image;
            }

            await _activityService.EditActivity(activityVm);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _activityService.DeleteActivity(id);
            return RedirectToAction("Index");
        }
    }
}
