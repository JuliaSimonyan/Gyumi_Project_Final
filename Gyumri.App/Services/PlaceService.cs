using Gyumri.Application.Interfaces;
using Gyumri.Common.ViewModel.Place;
using Microsoft.EntityFrameworkCore;
using Gyumri.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
                    DescriptionArm = p.DescriptionArm,
                    DescriptionRu = p.DescriptionRu,
                    PlaceName = p.PlaceName,
                    PlaceNameArm = p.PlaceNameArm,
                    PlaceNameRu = p.PlaceNameRu,
                    SubcategoryId = p.SubcategoryId,
                    Photo = p.Photo,
                    MinPrice = p.MinPrice,
                    MaxPrice = p.MaxPrice,
                    Raiting = p.Raiting,
                    ArticleId = p.ArticleId,
                    PlaceType = p.PlaceType,
                    Address = p.Address,
                    AddressArm = p.AddressArm,
                    AddressRu = p.AddressRu,
                })
                .ToListAsync();
        }

        public async Task<PlacesViewModel> GetPlaceById(int id)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.PlaceId == id);
            if (place == null) return null;

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
                SubcategoryId = place.SubcategoryId,
                MinPrice = place.MinPrice,
                MaxPrice = place.MaxPrice,
                Raiting = place.Raiting,
                ArticleId = place.ArticleId,
                PlaceType = place.PlaceType,
                Address = place.Address,
                AddressArm = place.AddressArm,
                AddressRu = place.AddressRu,
            };
        }

        public async Task<bool> AddPlace(AddEditPlaceViewModel model)
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
                SubcategoryId = model.SubcategoryId,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Raiting = model.Raiting,
                ArticleId = model.ArticleId,
                PlaceType = model.PlaceType,
                Address = model.Address,
                AddressArm = model.AddressArm,
                AddressRu = model.AddressRu,
            };

            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditPlace(AddEditPlaceViewModel model)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.PlaceId == model.Id);
            if (place == null) return false;

            place.PlaceName = model.PlaceName;
            place.PlaceNameArm = model.PlaceNameArm;
            place.PlaceNameRu = model.PlaceNameRu;
            place.Description = model.Description;
            place.DescriptionArm = model.DescriptionArm;
            place.DescriptionRu = model.DescriptionRu;
            place.Photo = model.Photo;
            place.SubcategoryId = model.SubcategoryId;
            place.MinPrice = model.MinPrice;
            place.MaxPrice = model.MaxPrice;
            place.Raiting = model.Raiting;
            place.ArticleId = model.ArticleId;
            place.PlaceType = model.PlaceType;
            place.Address = model.Address;
            place.AddressArm = model.AddressArm;
            place.AddressRu = model.AddressRu;

            _context.Places.Update(place);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlace(int id)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.PlaceId == id);
            if (place == null) return false;

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<List<PlacesViewModel>> GetPlacesBySubCategoryId(int id)
        {
            var places = _context.Places
                .Where(p => p.SubcategoryId == id)
                .Select(p => new PlacesViewModel
                {
                    Id = p.PlaceId,
                    PlaceName = p.PlaceName,
                    PlaceNameArm = p.PlaceNameArm,
                    PlaceNameRu = p.PlaceNameRu,
                    Description = p.Description,
                    DescriptionArm = p.DescriptionArm,
                    DescriptionRu = p.DescriptionRu,
                    Photo = p.Photo,
                    MinPrice = p.MinPrice,
                    MaxPrice = p.MaxPrice,
                    Raiting = p.Raiting,
                    ArticleId = p.ArticleId,
                    PlaceType = p.PlaceType,
                    Address = p.Address,
                    AddressArm = p.AddressArm,
                    AddressRu = p.AddressRu,
                })
                .ToListAsync();

            return places;
        }

        public async Task<Place> GetPlaceByArticleId(int? articleId)
        {
            return await _context.Places
        .Include(p => p.Article)
        .FirstOrDefaultAsync(p => p.ArticleId == articleId);
        }
    }
}
