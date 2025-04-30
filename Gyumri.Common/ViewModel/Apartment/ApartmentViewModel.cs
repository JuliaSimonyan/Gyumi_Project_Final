using Gyumri.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Gyumri.Common.ViewModel.Apartment
{
    public class ApartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public ApartmentType Type { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}
