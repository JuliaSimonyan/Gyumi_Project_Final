namespace Gyumri.Common.ViewModel.Subcategory
{
    public class EditSubcategoryViewModel
    {
        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public string? NameArm { get; set; }
        public string? NameRu { get; set; }
        public string Description { get; set; }
        public string? DescriptionArm { get; set; }
        public string? DescriptionRu { get; set; }
        public int CategoryId { get; set; }
    }
}
