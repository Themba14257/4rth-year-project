using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models.Content
{
    public class City
    {
        [Key]
        public string CityId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter city name")]
        [MaxLength(50)]
        [MinLength(3)]
        public string CityName { get; set; }
        public string SuburbId { get; set; }
        public virtual Suburb Suburb { get; set; }

    }
}
