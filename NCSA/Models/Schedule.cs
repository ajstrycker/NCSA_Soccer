using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCSA.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "Home Team")]
        public int HomeTeamID { get; set; }
        [Display(Name = "Away Team")]
        public int AwayTeam2ID { get; set; }
        [Display(Name = "Game Date and Time")]
        public DateTime GameDateTime { get; set; }
        [Display(Name = "Center Ref (Optional)")]
        public string CenterRef { get; set; }
        [Display(Name = "AR1 (Optional)")]
        public string AR1 { get; set; }
        [Display(Name = "AR2 (Optional)")]
        public string AR2 { get; set; }
    }
}