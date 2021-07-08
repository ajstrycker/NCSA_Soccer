using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class AdminTeamVM
    {
        public Team Team { get; set; }
        public Location Location { get; set; }
    }

    public class AdminTeamEditVM
    {
        public Team Team { get; set; }
        public Dictionary<int, string> GradeLevelList { get; set; }
        public Dictionary<int, string> Locations { get; set; }
    }
}