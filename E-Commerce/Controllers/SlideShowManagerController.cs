using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using PagedList;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlideShowManagerController : Controller
    {
        private Entities db = new Entities();

        // GET: SlideShowManager
        public ActionResult Index(string search, int? pageNo)
        {
            var slideshows = from s in db.SlideShows select s;
            if (!string.IsNullOrEmpty(search))
            {
                slideshows = slideshows.Where(s => s.Title.Contains(search) ||
                                              s.Caption.Contains(search) ||
                                              s.ImageAlt.Contains(search));
            }
            slideshows = slideshows.OrderBy(p => p.Id);
            return View(slideshows.ToPagedList(pageNo ?? 1, 5));
        }

        // GET: SlideShowManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlideShow slideShow = db.SlideShows.Find(id);
            if (slideShow == null)
            {
                return HttpNotFound();
            }
            return View(slideShow);
        }

        // GET: SlideShowManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlideShowManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageUrl,ImageAlt,Title,Caption")] SlideShow slideShow,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (db.SlideShows.Any(s => s.Title == slideShow.Title))
                {
                    ViewBag.Create = "This SlideShow is already exist !";
                    return View(slideShow);
                }
                if (upload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images/SlideShows"), upload.FileName);
                    upload.SaveAs(path);
                    slideShow.ImageUrl = upload.FileName;
                }
                else
                {
                    slideShow.ImageUrl = "No Image";
                }
                db.SlideShows.Add(slideShow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slideShow);
        }

        // GET: SlideShowManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlideShow slideShow = db.SlideShows.Find(id);
            if (slideShow == null)
            {
                return HttpNotFound();
            }
            return View(slideShow);
        }

        // POST: SlideShowManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,ImageAlt,Title,Caption")] SlideShow slideShow, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Content/Images/SlideShows"), slideShow.ImageUrl);
                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/SlideShows"), upload.FileName);
                    upload.SaveAs(path);
                    slideShow.ImageUrl = upload.FileName;
                }
                db.Entry(slideShow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slideShow);
        }

        // GET: SlideShowManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlideShow slideShow = db.SlideShows.Find(id);
            if (slideShow == null)
            {
                return HttpNotFound();
            }
            return View(slideShow);
        }

        // POST: SlideShowManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SlideShow slideShow = db.SlideShows.Find(id);
            string oldPath = Path.Combine(Server.MapPath("~/Content/Images/SlideShows"), slideShow.ImageUrl);
            System.IO.File.Delete(oldPath);
            db.SlideShows.Remove(slideShow);
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
