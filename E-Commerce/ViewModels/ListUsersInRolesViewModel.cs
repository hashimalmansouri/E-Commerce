using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce.ViewModels
{
    public class ListUsersInRolesViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string UserId { get; internal set; }
    }
}