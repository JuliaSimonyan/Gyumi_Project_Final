using Gyumri.Common.ViewModel.Subcategory;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gyumri.Application.Interfaces
{
    public interface ISubcategory
    {
        Task<bool> AddSubcategory(AddSubcategoryViewModel model);
        Task<bool> EditSubcategory(EditSubcategoryViewModel model);
        Task<bool> DeleteSubcategory(int subcategoryId);
        Task<List<SubcategoryViewModel>> GetAllSubcategories();
        Task<SubcategoryViewModel> GetSubcategoryById(int subcategoryId);
        Task<List<SubcategoryViewModel>> GetSubcategoriesByCategoryId(int categoryId);
    }
}
