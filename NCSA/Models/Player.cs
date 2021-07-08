using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCSA.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Parent First Name")]
        public string ParentFirstName { get; set; }
        [Display(Name = "Parent Last Name")]
        public string ParentLastName { get; set; }
        [Display(Name = "Player First Name")]
        public string PlayerFirstName { get; set; }
        [Display(Name = "Player Last Name")]
        public string PlayerLastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Shirt Size")]
        public string ShirtSize { get; set; }
        [Display(Name = "Address 1")]
        public string Addr1 { get; set; }
        [Display(Name = "Address 2")]
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public int TeamId { get; set; }
        [Display(Name = "Approves Pictures on Website")]
        public char ApprovesPictures { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Sign Up Date")]
        public DateTime SignUpDate { get; set; }
    }
}