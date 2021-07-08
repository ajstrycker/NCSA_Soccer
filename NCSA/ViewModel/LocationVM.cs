using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class LocationVM
    {
        public string TeamName { get; set; }
        public string TeamTitle { get; set; }
        public Location Location { get; set; }
    }
}