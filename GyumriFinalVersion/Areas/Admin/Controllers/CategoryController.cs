using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;


namespace Gyumri.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;

        public CategoryController(ICategory categoryService, ApplicationContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.CategoryList();
            return View(categories);
        }

        public IActionResult Add()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel category)
        {
            bool success = await _categoryService.Add(category);
            if (success)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Error adding category.");
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new EditCategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            var existingCategory = await _categoryService.GetCategoryById(model.CategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            bool success = await _categoryService.Edit(model);

            if (!success)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _categoryService.Delete(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
