using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Gyumri.Application.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationContext _context;

        public CategoryService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(AddCategoryViewModel category)
        {
            if (_context.Categories.Select(i => i.Name).Contains(category.Name) || category == null) return false;
            Category newCategory = new Category
            {
                Name = category.Name,
                NameArm = category.NameArm,
                NameRu = category.NameRu,

            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryViewModel>> CategoryList()
        {
            List<CategoryViewModel> categories = _context.Categories.Select(
                v => new CategoryViewModel
                {
                    CategoryId = v.CategoryId,
                    Name = v.Name,
                    NameArm = v.NameArm,
                    NameRu = v.NameRu,
                }
            ).ToList();
            return categories;
        }
        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                var categoryViewModel = new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    NameArm = category.NameArm,
                    NameRu = category.NameRu,
                };
                return categoryViewModel;
            }
            return null;
        }

        public async Task<bool> Edit(EditCategoryViewModel model)
        {
            try
            {
                var category = await _context.Categories.FindAsync(model.CategoryId);
                if (category == null)
                {
                    return false;
                }

                category.Name = model.Name;

                _context.Entry(category).State = EntityState.Modified;

                int rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryViewModel>> GetAllCategories()
        {
            return await _context.Categories
                            .Select(c => new CategoryViewModel
                            {
                                CategoryId = c.CategoryId,
                                Name = c.Name,
                                NameArm = c.NameArm,
                                NameRu = c.NameRu,
                            })
                            .ToListAsync();
        }
        public async Task SeedDefaultCategoriesAsync()
        {
            var defaultCategories = new List<Category>
            {
                 new Category()
    {
        Name = "Home",         
        NameArm = "Գլխավոր",
        NameRu = "Главная",
    },
                new Category()
                {
                    Name = "See & Do",
                    NameArm="Տեսնել և անել",
                    NameRu="Что посмотреть и чем заняться",
                },
                new Category()
                {
                    Name = "Eat & Drink",
                    NameArm="Ուտել-խմել",
                    NameRu="Есть и Пить",
                },
                new Category()
                {
                    Name = "Relax & Sleep",
                    NameArm="Հանգիստ և գիշերակաց",
                    NameRu="Отдых и ночёвка",
                },
                new Category()
                {
                    Name = "Live & Work",
                    NameArm="Կյանք և աշխատանք",
                    NameRu="Жизнь и работа",
                },

            };
            foreach (var item in defaultCategories)
            {
                if (!_context.Categories.Any(c => c.Name == item.Name))
                {
                    var category = new Category
                    {
                        Name = item.Name,
                        NameArm = item.NameArm,
                        NameRu = item.NameRu,
                    };

                    await _context.Categories.AddAsync(category);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
