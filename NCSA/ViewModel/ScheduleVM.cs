using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class ScheduleVM
    {
        public int CurrentMonth { get; set; }
        public string CurrentmonthName { get; set; }
        public List<Game> Games { get; set; }
    }

    public class Game
    {
        public DateTime GameDateTime { get; set; }
        public int GameID { get; set; }
        public string GameDate { get; set; }
        public string GameTime { get; set; }
        public string GradeLevel { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamDesc { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamDesc { get; set; }

        public Location Location { get; set; }
    }
}