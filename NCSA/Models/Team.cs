using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NCSA.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }
        [Required]
        [Display(Name = "Description (X/X Grade Boys/Girls)")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Town Name")]
        public string TownName { get; set; }
        [Required]
        public bool IsWalkertonTeam { get; set; }
        [Required]
        [Display(Name = "Grade Level")]
        public int GradeLevel { get; set; }
        [Required]
        [Display(Name = "Gender Of Team")]
        public string GenderOfTeam { get; set; }
        [Required]
        [Display(Name = "Head Coach")]
        public string Coach1 { get; set; }
        [Display(Name = "Head Coach Phone Number")]
        public string Coach1Phone { get; set; }
        [Display(Name = "Assistant Coach")]
        public string Coach2 { get; set; }
        public string ImagePath { get; set; }
        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        [Display(Name = "Practice Times (comma delimited list)")]
        public string PracticeTimes { get; set; }
        [Display(Name = "What to bring (comma delimited list)")]
        public string WhatToBring { get; set; }
    }
}