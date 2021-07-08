using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCSA.Models;

namespace NCSA.ViewModel
{
    public class UserVM
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string LockoutCount { get; set; }
        public bool IsLockedout { get; set; }
    }

    public class UserEditVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CurrentRole { get; set; }
        public IEnumerable<SelectListItem> RoleNames { get; set; }
    }
}