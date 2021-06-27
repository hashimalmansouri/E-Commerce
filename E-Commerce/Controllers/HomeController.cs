using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using PagedList;

namespace E_Commerce.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        ECommerceEntities db = new ECommerceEntities();
        public ActionResult Index(string search, int? pageNo)
        {
            IQueryable<Advert> products = db.Adverts
                .OrderByDescending(a => a.DateCreated);
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.AdvertName.Contains(search) ||
                                          p.Brand.BrandName.Contains(search) ||
                                          p.Genre.GenreName.Contains(search));
            }
            return View(products.ToPagedList(pageNo ?? 1, 6));
        }

        public ActionResult Contact(int? id)
        {
            if(id != null)
                ViewBag.Message = "Your contact information is sent successfully";
            return View();
        }
        [ChildActionOnly]
        public ActionResult SlideShow()
        {
            return PartialView(db.SlideShows.ToList());
        }
    }
}