using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using E_Commerce.ViewModels;

namespace E_Commerce.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        ECommerceEntities db = new ECommerceEntities();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the Advert from the database
            var addedAdvert = db.Adverts
                .Single(p => p.AdvertId == id);


            if (addedAdvert.Quantity > 0 && addedAdvert.TempQuantity > 0 && (addedAdvert.TempQuantity <= addedAdvert.Quantity))
            {
                // Add it to the shopping cart
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.AddToCart(addedAdvert);
                //////////////////////////////////
                addedAdvert.TempQuantity -= 1;
                db.Entry(addedAdvert).State = EntityState.Modified;
                db.SaveChanges();
                //////////////////////////////////
            }
            else
            {
                return RedirectToAction("QuantityOut");
            }

            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "Home");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the Advert to display confirmation
            string AdvertName = db.Carts
                .Single(item => item.RecordId == id).Advert.AdvertName;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(AdvertName) +
                          " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            // increase quantity
            return Json(results);
        }
        [ChildActionOnly]
        public ActionResult QuantityOut()
        {
            return View();
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}