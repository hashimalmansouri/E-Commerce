﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce.Models
{
    public class SlideShow
    {
        public int Id { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        [Display(Name = "Alternative Title")]
        public string ImageAlt { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Caption")]
        public string Caption { get; set; }
    }
}