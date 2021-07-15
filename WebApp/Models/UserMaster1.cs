using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [MetadataType(typeof(UserMasterMetadata))]
    public partial class UserMaster
    {
        public string Country { get; set; }
        public string State { get; set; }
    }

    public partial class UserMasterMetadata
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[0-9a-zA-Z]+", ErrorMessage ="Please Enter Alphabets and Digits only")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Range(18,100,ErrorMessage ="Age should be minimum 18")]
        public int Age { get; set; }
        [Required]
        [Display(Name ="Country")]
        public int CountryId { get; set; }
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }
    }
}