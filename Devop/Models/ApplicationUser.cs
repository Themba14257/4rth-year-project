using Devop.Enums;
using Devop.Models.Content;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Additional members are required
        [PersonalData]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [MaxLength(13)]
        [PersonalData]
        public string Idnumber { get; set; }

        [PersonalData]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [PersonalData]
        [MaxLength(50)]
        public string LastName { get; set; }
        [PersonalData]
        public string Title { get; set; }
        [PersonalData]
        public string Gender { get; set; }
        [PersonalData]
        [MaxLength(250)]
        public string AddressLine1 { get; set; }
        [PersonalData]
        [MaxLength(250)]
        public string AddressLine2 { get; set; }
        [PersonalData]
        [MaxLength(50)]
        public string CityId { get; set; }

        [PersonalData]
        [MaxLength(50)]
        public string SuburbId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [Display(Name = "Personel Details")]
        public string PersonelDetails
        {
            get
            {
                return LastName + " " + FirstName + ", " + Idnumber;
            }
        }
        public int Age
        {
            get
            {
                var now = DateTime.Today;
                var age = now.Year - Dob.Year;
                if (Dob > now.AddYears(-age)) age--;
                return age;
            }

        }
        [NotMapped]
        public string PatientIdOrDob
        {
            get
            {
                return string.Format("{0} ({1})", Idnumber, Dob);
            }
        }

        public virtual City City { get; set; }
        public virtual Suburb Suburb {get; set;}

    }
}
