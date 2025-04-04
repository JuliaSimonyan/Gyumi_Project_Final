using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public IActionResult Index()
        {
            var categories = _categoryService.CategoryList().Result;
            return View(categories);
        }

        public IActionResult Add()
        {
            return View(new AddCategoryViewModel());
        }

        [HttpPost]
        public  async Task<IActionResult> Add(AddCategoryViewModel category)
        {
           
                bool success = (await _categoryService.Add(category));
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error adding category.");
        
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
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
        public IActionResult Edit(EditCategoryViewModel model)
        {

            //if (model == null)
            //{
            //    return View(model);
            //}

            //var category = _categoryService.GetCategoryById(model.CategoryId);
            //if (category == null)
            //{
            //    return NotFound();
            //}

            //_categoryService.Edit(category.Result);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool success = _categoryService.Delete(id).Result;
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
