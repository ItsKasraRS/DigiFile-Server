using BackEnd.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Core.DTOs.Product
{
    public class GetHomeProducts
    {
        public string Image { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }

    public class ProductItemDTO
    {
        public long Id { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }

    public class FilterProductsDTO : BasePaging
    {
        public string Title { get; set; }

        public List<ProductItemDTO> Products { get; set; }

        public int? Categories { get; set; }

        public ProductSort? SortBy { get; set; }

        public FilterProductsDTO SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            PageCount = paging.PageCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            ActivePage = paging.ActivePage;
            return this;
        }

        public FilterProductsDTO SetProduct(List<ProductItemDTO> products)
        {
            Products = products;
            return this;
        }
    }

    public enum ProductSort
    {
        NotSelected,
        PriceAsc,
        PriceDesc,
        Latest,
        Popular
    }

    public class ProductDetailsDTO
    {
        [Key]
        public long Id { get; set; }

        public int CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

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
        public decimal Price { get; set; }

        [Display(Name = "Source file")]
        public string SourceFile { get; set; }


        public List<ProductInfoDTO> ProductInfos { get; set; }
        public List<ProductGallery> ProductPictures { get; set; }


    }

    public class ProductInfoDTO
    {
        public string InfoTitle { get; set; }

        public string InfoValue { get; set; }
    }
}