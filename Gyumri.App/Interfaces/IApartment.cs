using Gyumri.Common.ViewModel.Apartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.App.Interfaces
{
    public interface IApartment
    {
        Task<List<ApartmentViewModel>> GetAllApartments();
        Task<ApartmentViewModel> GetApartmentById(int id);
        Task<bool> AddApartment(ApartmentViewModel model);
        Task<bool> EditApartment(ApartmentViewModel model);
        Task DeleteApartment(int id);
    }
}
