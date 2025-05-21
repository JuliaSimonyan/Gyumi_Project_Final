using Gyumri.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Common.ViewModel.Place
{
    public class DeletePlaceViewModel
    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public string PlaceNameArm { get; set; }
        public string PlaceNameRu { get; set; }
        public string Description { get; set; }
        public string DescriptionArm { get; set; }
        public string DescriptionRu { get; set; }
        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public int? Raiting { get; set; }
        public int? ArticleId { get; set; }
        public int SubcategoryId { get; set; }

        public string? Address { get; set; }
        public string? AddressArm { get; set; }
        public string? AddressRu { get; set; }

        public PlaceType? PlaceType { get; set; }
    }
}
