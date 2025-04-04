using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Subcategory;
using Microsoft.AspNetCore.Mvc;

namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategory _subcategoryService;
        private readonly ICategory _categoryService;

        public SubcategoryController(ISubcategory subcategoryService, ICategory categoryService)
        {
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var subcategories = await _subcategoryService.GetAllSubcategories();

            var subcategoryViewModels = subcategories.Select(s => new SubcategoryViewModel
            {
                SubcategoryId = s.SubcategoryId,
                Name = s.Name,
                CategoryName = s.CategoryName
            }).ToList();

            return View(subcategoryViewModels);
        }


        public IActionResult Add()
        {
            ViewBag.Categories = _categoryService.GetAllCategories().Result;
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddSubcategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAllCategories().Result;
                return View(model);
            }

            var result = _subcategoryService.AddSubcategory(model).Result;
            if (result)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add subcategory.");
            ViewBag.Categories = _categoryService.GetAllCategories().Result;
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subcategory = _subcategoryService.GetSubcategoryById(id).Result;

            if (subcategory == null)
            {
                return NotFound();
            }

            var model = new EditSubcategoryViewModel
            {
                SubcategoryId = subcategory.SubcategoryId,
                Name = subcategory.Name,
                CategoryId = subcategory.CategoryId
            };

            ViewBag.Categories = _categoryService.GetAllCategories().Result;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditSubcategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAllCategories().Result;
                return View(model);
            }

            var result = _subcategoryService.EditSubcategory(model).Result;
            if (result)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to edit subcategory.");
            ViewBag.Categories = _categoryService.GetAllCategories().Result;
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool success = _subcategoryService.DeleteSubcategory(id).Result;
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
