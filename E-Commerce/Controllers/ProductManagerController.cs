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
    public class ProductManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductManager
        public ActionResult Index(string search, int? pageNo)
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Genre);
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            products = products.OrderBy(p => p.ProductId);
            return View(products.ToPagedList(pageNo ?? 1, 5));
        }

        // GET: ProductManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductManager/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: ProductManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,ImageUrl,Description,Quantity,TempQuantity,BrandId,GenreId")] Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Any(n => n.ProductName == product.ProductName))
                {
                    ViewBag.Create = "This Product is already exist !";
                    ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
                    ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
                    return View(product);
                }
                if (upload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), upload.FileName);
                    upload.SaveAs(path);
                    product.ImageUrl = upload.FileName;
                }
                else
                {
                    product.ImageUrl = "No Image";
                }
                product.TempQuantity = product.Quantity;
                product.DateCreated = DateTime.Today.Date;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // GET: ProductManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // POST: ProductManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,ImageUrl,Description,Quantity,TempQuantity,BrandId,GenreId")] Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Content/Images/Products"), product.ImageUrl);
                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Products"), upload.FileName);
                    upload.SaveAs(path);
                    product.ImageUrl = upload.FileName;
                }
                product.TempQuantity = product.Quantity;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", product.BrandId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", product.GenreId);
            return View(product);
        }

        // GET: ProductManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
