using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Address
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Display(Name = "State")]
        public string StateName { get; set; }
    }
    [MetadataType(typeof(CountryMetadata))]
    public partial class Country
    {
    }
    public partial class CountryMetadata
    {
        [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        [Required]
        [Display(Name = "Phone Code")]
        public string CountryCode { get; set; }

    }
    [MetadataType(typeof(StateMetadata))]
    public partial class State
    {
    }
    public partial class StateMetadata
    {
        [Required]
        [Display(Name = "State")]
        public string StateName { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Nullable<int> CountryId { get; set; }
    }

}