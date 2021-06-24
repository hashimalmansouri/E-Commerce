using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Models
{
    public class Genre
    {
        [ScaffoldColumn(false)]
        public int GenreId { get; set; }
        [Required(ErrorMessage = "Genre Name is Required")]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}