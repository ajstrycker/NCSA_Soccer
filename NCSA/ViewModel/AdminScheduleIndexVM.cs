using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class AdminScheduleIndexVM
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string GameDateTime { get; set; }
        public string CenterRef { get; set; }
        public string ARs { get; set; }
    }
}