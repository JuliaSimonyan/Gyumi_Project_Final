using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Place;
using Microsoft.EntityFrameworkCore;
using Gyumri.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gyumri.Application.Services
{
    public class PlaceService : IPlace
    {
        private readonly ApplicationContext _context;

        public PlaceService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<PlacesViewModel>> GetAllPlaces()
        {
            return await _context.Places
                .Select(p => new PlacesViewModel
                {
                    Id = p.PlaceId,
                    Description = p.Description,
                    PlaceName = p.PlaceName,
                    PlaceNameArm = p.PlaceNameArm,
                    PlaceNameRu = p.PlaceNameRu,
                    SubcategoryId = p.SubcategoryId,
                    Photo = p.Photo
                })
                .ToListAsync();
        }


        public PlacesViewModel GetPlaceById(int id)
        {
            var place = _context.Places.FirstOrDefault(p => p.PlaceId == id);
            if (place == null)
                return null;

            return new PlacesViewModel
            {
                Id = place.PlaceId,
                PlaceName = place.PlaceName,
                PlaceNameArm = place.PlaceNameArm,
                PlaceNameRu = place.PlaceNameRu,
                Description = place.Description,
                DescriptionArm = place.DescriptionArm,
                DescriptionRu = place.DescriptionRu,
                Photo = place.Photo,
                SubcategoryId = place.SubcategoryId
            };
        }

        public bool AddPlace(AddEditPlaceViewModel model)
        {
            var place = new Place
            {
                PlaceName = model.PlaceName,
                PlaceNameArm = model.PlaceNameArm,
                PlaceNameRu = model.PlaceNameRu,
                Description = model.Description,
                DescriptionArm = model.DescriptionArm,
                DescriptionRu = model.DescriptionRu,
                Photo = model.Photo,
                SubcategoryId = model.SubcategoryId
            };

            _context.Places.Add(place);
            _context.SaveChanges();
            return true;
        }

        public bool EditPlace(AddEditPlaceViewModel model)
        {
            var place = _context.Places.FirstOrDefault(p => p.PlaceId == model.Id);
            if (place == null) return false;

            place.PlaceName = model.PlaceName;
            place.PlaceNameArm = model.PlaceNameArm;
            place.PlaceNameRu = model.PlaceNameRu;
            place.Description = model.Description;
            place.DescriptionArm = model.DescriptionArm;
            place.DescriptionRu = model.DescriptionRu;
            place.Photo = model.Photo;
            place.SubcategoryId = model.SubcategoryId;

            _context.Places.Update(place);
            _context.SaveChanges();
            return true;
        }

        public bool DeletePlace(int id)
        {
            var place = _context.Places.FirstOrDefault(p => p.PlaceId == id);
            if (place == null) return false;

            _context.Places.Remove(place);
            _context.SaveChanges();
            return true;
        }

    }
}
