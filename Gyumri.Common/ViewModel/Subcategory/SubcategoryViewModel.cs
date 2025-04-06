using Gyumri.Common.ViewModel.Place;

namespace Gyumri.Common.ViewModel.Subcategory
{
    public class SubcategoryViewModel
    {
        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public string? NameArm { get; set; }
        public string? NameRu { get; set; }
        public string Description { get; set; }
        public string? DescriptionArm { get; set; }
        public string? DescriptionRu { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryNameArm { get; set; }
        public string? CategoryNameRu { get; set; }
        public List<PlacesViewModel> Places { get; set; }


    }
}
