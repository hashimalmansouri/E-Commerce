using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using PagedList;

namespace E_Commerce.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        ECommerceEntities db = new ECommerceEntities();
        public ActionResult Index(int? brandId, int? genreId, string search, int? pageNo)
        {
            var products = from p in db.Adverts select p;
            if (brandId != null)
            {
                products = products.Where(b => b.BrandId == brandId);
                ViewBag.BrandId = brandId;
                ViewBag.Name = db.Brands.SingleOrDefault(x => x.BrandId == brandId).BrandName;
            }
            if (genreId != null)
            {
                products = products.Where(g => g.GenreId == genreId);
                ViewBag.GenreId = genreId;
                ViewBag.Name = db.Genres.SingleOrDefault(x => x.GenreId == genreId).GenreName;
            }
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.AdvertName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            products = products.OrderByDescending(p => p.AdvertId);
            return View(products.ToPagedList(pageNo ?? 1, 6));
        }
        public ActionResult ProductDetails(int? id)
        {
            var product = db.Adverts.Find(id);
            return View(product);
        }
        [ChildActionOnly]
        public ActionResult BrandsMenu()
        {
            return PartialView(db.Brands.ToList());
        }
        [ChildActionOnly]
        public ActionResult GenresMenu()
        {
            return PartialView(db.Genres.ToList());
        }
    }
}