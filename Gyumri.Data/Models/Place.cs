using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gyumri.Data.Models
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }//en
        public string? PlaceNameArm { get; set; }
        public string? PlaceNameRu { get; set; }
        public string Description { get; set; }
        public string? DescriptionArm { get; set; }
        public string? DescriptionRu { get; set; }
        //public string Address { get; set; }
        //public string? AddressArm { get; set; }
        //public string? AddressRu { get; set; }

        public string Photo { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public int? Raiting { get; set; }

        [ForeignKey("Subcategory")]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        [ForeignKey("Article")]
        public int? ArticleId { get; set; }
        public Article? Article { get; set; }
    }
}
