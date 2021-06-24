using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Commerce.Models;
using E_Commerce.ViewModels;

namespace E_Commerce.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int Id { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}