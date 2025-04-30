using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Data.Models
{
    public class Apartment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ApartmentType Type { get; set; }

        [Required]
        public int MinPrice { get; set; }

        [Required]
        public int MaxPrice { get; set; }

        public string Description { get; set; }

        public string Image { get; set; } 
    }
}
