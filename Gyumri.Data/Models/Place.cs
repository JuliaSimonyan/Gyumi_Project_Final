﻿using System.ComponentModel.DataAnnotations.Schema;

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
        public string Photo { get; set; }

        [ForeignKey("Subcategory")]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
