using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Data.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string OpeningHours { get; set; }

        public int PriceMin { get; set; }

        public int PriceMax { get; set; }

        public ActivityType ActivityType { get; set; }

        public string Image { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }
    }
}
