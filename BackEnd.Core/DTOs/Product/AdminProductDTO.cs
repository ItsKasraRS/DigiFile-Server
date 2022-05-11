using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BackEnd.Core.DTOs.Product
{
    public class AdminFilterProducts
    {
        public List<DataLayer.Entities.Product.Product> Products { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
    }

    public class AddProductDTO
    {
        [Display(Name = "Title")]
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        //public string Image { get; set; }

        //public string SourceFile { get; set; }

        [Display(Name = "Product price")]
        [Required]
        public decimal Price { get; set; }

        public int Category { get; set; }
        public int? SubCategory { get; set; }

        public string SelectedImage { get; set; }

        public IFormFile SelectedSourceFile { get; set; }

    }

    public class EditProductDTO
    {
        public long Id { get; set; }

        [Display(Name = "Title")]
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public string Image { get; set; }

        public string SourceFile { get; set; }

        [Display(Name = "Product price")]
        [Required]
        public decimal Price { get; set; }

        public int Category { get; set; }
        public int? SubCategory { get; set; }

        public string SelectedImage { get; set; }

        public IFormFile SelectedSourceFile { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
