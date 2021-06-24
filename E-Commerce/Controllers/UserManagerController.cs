using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: UserManager
        public ActionResult Index(string search, int? pageNo)
        {
            var users = from u in db.Users select u;
            if (!String.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.UserName.Contains(search) ||
                                    u.Email == search ||
                                    u.PhoneNumber == search);
            }
            users = users.OrderBy(u => u.Id);
            return View(users.ToPagedList(pageNo ?? 1, 5));
        }

        // GET: UserManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: UserManager/Create
        public ActionResult Create()
        {
            ViewBag.AccountType = new SelectList(db.Roles.Where(a => !a.Name.Contains("Admin")).ToList(), "Name", "Name");
            return View();
        }

        // POST: UserManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser, string AccountType)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AccountType = new SelectList(db.Roles.Where(a =>
                !a.Name.Contains("Admin")).ToList(), "Name", "Name");
                if (db.Users.Any(n => n.UserName == applicationUser.UserName) && db.Users.Any(n =>
                n.Email == applicationUser.Email))
                {
                    ViewBag.Create1 = applicationUser.UserName;
                    ViewBag.Create2 = "UserName is already taken !";
                    ViewBag.Create3 = applicationUser.Email;
                    ViewBag.Create4 = "Email is already taken !";
                }
                else if (db.Users.Any(n => n.UserName == applicationUser.UserName))
                {
                    ViewBag.Create1 = applicationUser.UserName;
                    ViewBag.Create2 = "UserName is already taken !";
                }
                else if (db.Users.Any(n => n.Email == applicationUser.Email))
                {
                    ViewBag.Create1 = applicationUser.Email;
                    ViewBag.Create2 = "Email is already taken !";
                }
                else
                {
                    var result = await UserManager.CreateAsync(applicationUser,
                    applicationUser.PasswordHash);
                    await UserManager.AddToRolesAsync(applicationUser.Id, AccountType);
                    return RedirectToAction("Index");
                }
            }
            return View(applicationUser);
        }
        // GET: UserManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: UserManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser, string id)
        {
            if (ModelState.IsValid)
            {
                var UserID = id;
                var CurrentUser = db.Users.Where(a => a.Id == UserID).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    if ((db.Users.Any(n => n.UserName == applicationUser.UserName) &&
                    applicationUser.UserName != CurrentUser.UserName) && (db.Users.Any(n => n.Email ==
                    applicationUser.Email) && applicationUser.Email != CurrentUser.Email))
                    {
                        ViewBag.Edit1 = applicationUser.UserName;
                        ViewBag.Edit2 = "Username is already taken !";
                        ViewBag.Edit3 = applicationUser.Email;
                        ViewBag.Edit4 = "Email is already taken !";
                    }
                    else if (db.Users.Any(n => n.UserName == applicationUser.UserName) &&
                    applicationUser.UserName != CurrentUser.UserName)
                    {
                        ViewBag.Edit1 = applicationUser.UserName;
                        ViewBag.Edit2 = "Username is already taken !";
                    }
                    else if (db.Users.Any(n => n.Email == applicationUser.Email) &&
                    applicationUser.Email != CurrentUser.Email)
                    {
                        ViewBag.Edit1 = applicationUser.Email;
                        ViewBag.Edit2 = "Email is already taken !";
                    }
                    else
                    {
                        var newPasswordHash =
                        UserManager.PasswordHasher.HashPassword(applicationUser.PasswordHash);
                        CurrentUser.UserName = applicationUser.UserName;
                        CurrentUser.Email = applicationUser.Email;
                        CurrentUser.EmailConfirmed = applicationUser.EmailConfirmed;
                        CurrentUser.PasswordHash = newPasswordHash;
                        CurrentUser.PhoneNumber = applicationUser.PhoneNumber;
                        CurrentUser.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                        CurrentUser.LockoutEnabled = applicationUser.LockoutEnabled;
                        CurrentUser.SecurityStamp = applicationUser.SecurityStamp;
                        CurrentUser.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                        CurrentUser.LockoutEndDateUtc = applicationUser.LockoutEndDateUtc;
                        CurrentUser.AccessFailedCount = applicationUser.AccessFailedCount;
                        CurrentUser.FirstName = applicationUser.FirstName;
                        CurrentUser.LastName = applicationUser.LastName;
                        db.Entry(CurrentUser).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(applicationUser);
        }
        // GET: UserManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
