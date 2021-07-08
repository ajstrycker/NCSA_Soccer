using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class RefereeVM
    {
        public string UserFullName { get; set; }
        public List<RefereeGame> GamesAssignedTo { get; set; }
        public List<RefereeGame> OpenGames { get; set; }
    }

    public class RefereeGame
    {
        public int GameId { get; set; }
        public string GameDescription { get; set; }
        public string GameDateTime { get; set; }
        public string CenterRef { get; set; }
        public string AR1 { get; set; }
        public string AR2 { get; set; }
    }
}