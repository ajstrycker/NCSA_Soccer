using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class PlayerVM
    {
        public List<PlayerIndexVM> Players { get; set; }
        public Dictionary<string, string> ListOfGrades { get; set; }
    }

    public class PlayerIndexVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TeamDesc { get; set; }
    }

    public class PlayerDetailsVM
    {
        public Player player { get; set; }
        public string Team { get; set; }
    }
}