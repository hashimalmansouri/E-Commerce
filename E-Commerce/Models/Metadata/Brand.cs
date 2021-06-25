using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public partial class Brand
    {
        public class Metadata
        {
            [ScaffoldColumn(false)]
            public int BrandId { get; set; }
            [Required(ErrorMessage = "Brand Name is Required")]
            [Display(Name = "Brand Name")]
            public string BrandName { get; set; }
        }
    }
}