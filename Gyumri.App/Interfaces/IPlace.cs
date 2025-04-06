using Gyumri.Common.ViewModel.Place;
using System.Collections.Generic;

namespace Gyumri.Application.Interfaces
{
    public interface IPlace
    {
        Task<List<PlacesViewModel>> GetAllPlaces();
        Task<PlacesViewModel> GetPlaceById(int id);
        Task<bool> AddPlace(AddEditPlaceViewModel model);
        Task<bool> EditPlace(AddEditPlaceViewModel model);
        Task<bool> DeletePlace(int id);
        Task<List<PlacesViewModel>> GetPlacesBySubCategoryId(int id);
    }
}
