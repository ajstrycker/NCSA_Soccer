using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class HomeViewModel
    {
        public List<Team> Teams { get; set; }
        public List<GameVM> Games { get; set; }
    }
}