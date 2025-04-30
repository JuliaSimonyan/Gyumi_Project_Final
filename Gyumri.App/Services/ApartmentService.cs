using Gyumri.App.Interfaces;
using Gyumri.Common.ViewModel.Apartment;
using Gyumri.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.App.Services
{
    public class ApartmentService : IApartment
    {
        private readonly ApplicationContext _context;

        public ApartmentService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> AddApartment(ApartmentViewModel model)
        {
            var apartment = new Apartment
            {
                Name = model.Name,
                Type = model.Type,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Description = model.Description,
                Image = model.Image
            };

            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteApartment(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EditApartment(ApartmentViewModel model)
        {
            var apartment = await _context.Apartments.FindAsync(model.Id);
            if (apartment != null)
            {
                apartment.Name = model.Name;
                apartment.Type = model.Type;
                apartment.MinPrice = model.MinPrice;
                apartment.MaxPrice = model.MaxPrice;
                apartment.Description = model.Description;
                apartment.Image = model.Image;

                _context.Apartments.Update(apartment);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<ApartmentViewModel>> GetAllApartments()
        {
            return await _context.Apartments
               .Select(a => new ApartmentViewModel
               {
                   Id = a.Id,
                   Name = a.Name,
                   Type = a.Type,
                   MinPrice = a.MinPrice,
                   MaxPrice = a.MaxPrice,
                   Description = a.Description,
                   Image = a.Image
               })
               .ToListAsync();
        }

        public async Task<ApartmentViewModel> GetApartmentById(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return null;
            }

            return new ApartmentViewModel
            {
                Id = apartment.Id,
                Name = apartment.Name,
                Type = apartment.Type,
                MinPrice = apartment.MinPrice,
                MaxPrice = apartment.MaxPrice,
                Description = apartment.Description,
                Image = apartment.Image
            };
        }
    }
}
