using Gyumri.Common.ViewModel.Category;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Application.Interfaces
{
    public interface ICategory
    {
        Task<List<CategoryViewModel>> CategoryList();
        Task<bool> Add(AddCategoryViewModel category);
        Task<CategoryViewModel> GetCategoryById(int id);
        Task<bool> Edit(EditCategoryViewModel model);
        Task<bool> Delete(int id);
        Task<List<CategoryViewModel>> GetAllCategories();
        Task SeedDefaultCategoriesAsync();

    }
}
