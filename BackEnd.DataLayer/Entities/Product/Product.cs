using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.Product
{
    public class Product
    {
        [Key]
        public long Id { get; set; }


        [Display(Name = "Title")]
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public int Like { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Image { get; set; }

        [Display(Name = "Product price")]
        [Required]
        public int Price { get; set; }

        public bool IsDelete { get; set; }


        #region Refrences

        public List<ProductInfo> ProductInfos { get; set; }

        public List<Comment> Comments { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        #endregion
    }

    public class ProductInfo
    {
        [Key]
        public long Id { get; set; }

        public long ProductId { get; set; }

        [Display(Name = "Info title")]
        [Required]
        [MaxLength(200)]
        public string InfoTitle { get; set; }

        [Display(Name = "Info value")]
        [Required]
        [MaxLength(500)]
        public string InfoValue { get; set; }

        public bool IsDelete { get; set; }

        #region Refrences

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        #endregion
    }

    public class ProductGallery
    {
        [Key]
        public long Id { get; set; }

        public string GalleryImage { get; set; }

        public bool IsDelete { get; set; }

    }
}
