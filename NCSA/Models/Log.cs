using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NCSA.Models
{
    public class Log
    {
        [Key]
        public int ID { get; set; }
        public string Controller { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public DateTime Date { get; set; }
    }
}