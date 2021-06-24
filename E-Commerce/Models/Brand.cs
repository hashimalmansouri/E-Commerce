using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Models
{
    public class Brand
    {
        [ScaffoldColumn(false)]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Brand Name is Required")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}