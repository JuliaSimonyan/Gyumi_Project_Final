using Gyumri.Common.ViewModel.Place;
using System.Collections.Generic;

namespace Gyumri.Application.Interfaces
{
    public interface IPlace
    {
        Task<List<PlacesViewModel>> GetAllPlaces();
        PlacesViewModel GetPlaceById(int id);
        bool AddPlace(AddEditPlaceViewModel model);
        bool EditPlace(AddEditPlaceViewModel model);
        bool DeletePlace(int id);
    }
}
