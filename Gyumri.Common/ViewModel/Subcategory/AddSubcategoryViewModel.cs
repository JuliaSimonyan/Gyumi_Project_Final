using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyumri.Common.ViewModel.Subcategory
{
    public class AddSubcategoryViewModel
    {
        public string? Name { get; set; }
        public string? NameArm { get; set; }
        public string? NameRu { get; set; }
        public string Description { get; set; }
        public string? DescriptionArm { get; set; }
        public string? DescriptionRu { get; set; }
        public int CategoryId { get; set; }
    }
}
