using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class AdminScheduleEditVM
    {
        public Schedule Game { get; set; }
        public Dictionary<int, string> Teams { get; set; }
    }
}