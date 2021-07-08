using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCSA.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Latitude (Use Google Maps to get exact location)")]
        public string Latitude { get; set; }
        [Required]
        [Display(Name = "Latitude (Use Google Maps to get exact location)")]
        public string Longitude { get; set; }
    }
}