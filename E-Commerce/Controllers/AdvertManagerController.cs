using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using System.IO;
using PagedList;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdvertManagerController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: AdvertManager
        public ActionResult Index(string search, int? pageNo)
        {
            var Adverts = db.Adverts.Include(p => p.Brand).Include(p => p.Genre);
            if (!String.IsNullOrEmpty(search))
            {
                Adverts = Adverts.Where(p => p.AdvertName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            Adverts = Adverts.OrderBy(p => p.AdvertId);
            return View(Adverts.ToPagedList(pageNo ?? 1, 5));
        }

        // GET: AdvertManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert Advert = db.Adverts.Find(id);
            if (Advert == null)
            {
                return HttpNotFound();
            }
            return View(Advert);
        }

        // GET: AdvertManager/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: AdvertManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdvertId,AdvertName,Price,ImageUrl,Description,Quantity,TempQuantity,BrandId,GenreId")] Advert Advert, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (db.Adverts.Any(n => n.AdvertName == Advert.AdvertName))
                {
                    ViewBag.Create = "This Advert is already exist !";
                    ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", Advert.BrandId);
                    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", Advert.GenreId);
                    return View(Advert);
                }
                if (upload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Adverts"), upload.FileName);
                    upload.SaveAs(path);
                    Advert.ImageUrl = upload.FileName;
                }
                else
                {
                    Advert.ImageUrl = "No Image";
                }
                Advert.TempQuantity = Advert.Quantity;
                Advert.DateCreated = DateTime.Today.Date;
                db.Adverts.Add(Advert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", Advert.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", Advert.GenreId);
            return View(Advert);
        }

        // GET: AdvertManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert Advert = db.Adverts.Find(id);
            if (Advert == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", Advert.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", Advert.GenreId);
            return View(Advert);
        }

        // POST: AdvertManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdvertId,AdvertName,Price,ImageUrl,Description,Quantity,TempQuantity,BrandId,GenreId")] Advert Advert, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Content/Images/Adverts"), Advert.ImageUrl);
                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Adverts"), upload.FileName);
                    upload.SaveAs(path);
                    Advert.ImageUrl = upload.FileName;
                }
                Advert.TempQuantity = Advert.Quantity;
                db.Entry(Advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", Advert.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", Advert.GenreId);
            return View(Advert);
        }

        // GET: AdvertManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert Advert = db.Adverts.Find(id);
            if (Advert == null)
            {
                return HttpNotFound();
            }
            return View(Advert);
        }

        // POST: AdvertManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert Advert = db.Adverts.Find(id);
            db.Adverts.Remove(Advert);
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
