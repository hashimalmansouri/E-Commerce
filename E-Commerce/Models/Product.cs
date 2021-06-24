using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [Display(Name = "Product Name")]
        [StringLength(200)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        [Range(0, 5000)]
        public decimal Price { get; set; }
        //[Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Range(0, 100)]
        public int Quantity { get; set; }
        [ScaffoldColumn(false)]
        public int TempQuantity { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? DateCreated { get; set; }
        //[Required(ErrorMessage = "Brand is required")]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        //[Required(ErrorMessage = "Genre is required")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Genre Genre { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}