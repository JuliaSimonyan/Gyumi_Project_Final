using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Common.ViewModel.Place
{
    public class AddEditPlaceViewModel
    {
        public int Id { get; set; } 
        public string PlaceName { get; set; }
        public string? PlaceNameArm { get; set; }
        public string? PlaceNameRu { get; set; }
        public string Description { get; set; }
        public string? DescriptionArm { get; set; }
        public string? DescriptionRu { get; set; }
        public string Photo { get; set; }
        public int SubcategoryId { get; set; }
    }

}
