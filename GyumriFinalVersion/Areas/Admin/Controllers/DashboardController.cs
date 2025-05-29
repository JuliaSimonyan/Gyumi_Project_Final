using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OptimizeStaticImages()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OptimizeStaticImages([FromServices] ImageOptimizerService optimizer)
        {
            try
            {
                optimizer.OptimizeSpecificImageFolders();
                TempData["Message"] = "Static images optimized and originals deleted.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

    }
}
