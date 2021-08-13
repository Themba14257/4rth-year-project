using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models.Content
{
    public class Suburb
    {
        [Key]
        public string SuburbId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Suburb name")]
        [Display(Name = "Suburb")]
        [MinLength(3)]
        public string SuburbName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select province name")]
        public string CityId { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
