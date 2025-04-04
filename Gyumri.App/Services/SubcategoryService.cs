using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Subcategory;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Application.Services
{
    public class SubcategoryService : ISubcategory
    {
        private readonly ApplicationContext _context;

        public SubcategoryService(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<bool> AddSubcategory(AddSubcategoryViewModel model)
        {
            if (model == null) return false;
            Subcategory subcategory = new()
            {
                Name = model.Name,
                NameArm = model.NameArm,
                NameRu = model.NameRu,
                CategoryId = model.CategoryId,
            };
            await _context.AddAsync(subcategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSubcategory(int id)
        {
            var entity = await _context.Subcategories.FirstOrDefaultAsync(s => s.SubcategoryId == id);
            if (entity == null) return false;

            _context.Subcategories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> EditSubcategory(EditSubcategoryViewModel model)
        {
            if (model == null) return false;

            var subcategory = await _context.Subcategories.FindAsync(model.SubcategoryId);
            if (subcategory == null) return false;

            subcategory.Name = model.Name;
            subcategory.NameArm = model.NameArm;
            subcategory.NameRu = model.NameRu;
            subcategory.CategoryId = model.CategoryId;

            _context.Subcategories.Update(subcategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SubcategoryViewModel>> GetAllSubcategories()
        {
            return await _context.Subcategories
                .Include(s => s.Category)
                .Select(s => new SubcategoryViewModel
                {
                    SubcategoryId = s.SubcategoryId,
                    Name = s.Name,
                    NameArm = s.NameArm,
                    NameRu = s.NameRu,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name
                })
                .ToListAsync();
        }


        public async Task<SubcategoryViewModel> GetSubcategoryById(int subcategoryId)
        {
            var subcategory = await _context.Subcategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SubcategoryId == subcategoryId);

            if (subcategory == null) return null;

            return new SubcategoryViewModel
            {
                SubcategoryId = subcategory.SubcategoryId,
                Name = subcategory.Name,
                NameArm = subcategory.NameArm,
                NameRu = subcategory.NameRu,
                CategoryId = subcategory.CategoryId,
                CategoryName = subcategory.Category.Name,
                CategoryNameArm = subcategory.Category.NameArm,
                CategoryNameRu = subcategory.Category.NameRu
            };
        }
        public async Task<List<SubcategoryViewModel>> GetSubcategoriesByCategoryId(int categoryId)
        {
            return await _context.Subcategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new SubcategoryViewModel
                {
                    SubcategoryId = s.SubcategoryId,
                    Name = s.Name,
                    NameArm = s.NameArm,
                    NameRu = s.NameRu,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name,
                    CategoryNameArm = s.Category.NameArm,
                    CategoryNameRu = s.Category.NameRu
                })  
                .ToListAsync();
        }
    }
}

