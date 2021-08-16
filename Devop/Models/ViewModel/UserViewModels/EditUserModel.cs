using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models.ViewModel.UserViewModels
{
    public class EditUserModel
    {
        //Additional members are required

        public string Id { get; set; }
        [PersonalData]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [MaxLength(50)]
        [PersonalData]

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
        public string Idnumber { get; set; }

        [PersonalData]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [PersonalData]
        [MaxLength(50)]
        public string LastName { get; set; }
        [PersonalData]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }
      
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
        [PersonalData]
        public string PostalCode { get; set; }
    }
}
