using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using System.Data.Entity;
using PagedList;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersInRolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: UsersInRoles
        public ActionResult Index(int? pageNo, string search)
        {
            var modelList = new List<ListUsersInRolesViewModel>();
            var role = db.Roles.Include(x => x.Users).ToList();
            foreach (var r in role)
            {
                foreach (var u in r.Users)
                {
                    var user = db.Users.Find(u.UserId);
                    var obj = new ListUsersInRolesViewModel
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        RoleName = r.Name
                    };
                    modelList.Add(obj);
                }
            }
            return View(modelList.ToPagedList(pageNo ?? 1, 5));
        }
    }
}