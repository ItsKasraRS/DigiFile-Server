﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Product;
using BackEnd.DataLayer.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace BackEnd.Core.Interfaces
{
    public interface IProductService
    {
        // Site Services

        Task<List<GetHomeProducts>> GetLatestProducts();
        Task<List<GetHomeProducts>> GetPopularProducts();
        Task<FilterProductsDTO> FilterProducts(FilterProductsDTO filter);
        Task<ProductDetailsDTO> GetProductDetails(long id);
        Task<Product> GetProductForUserOrder(long productId);
        Task<List<ProductGallery>> GetProductGallery(long productId);
    }

    public interface IAdminProductService
    {
        // Admin Services
        
        Task<AdminFilterProducts> GetProductsForAdmin(int pageId = 1, string filterTitle = "");
        Task AddProductForAdmin(AddProductDTO model, IFormFile sourceFile);
        Task<EditProductDTO> GetProductForAdmin(long id);
        Task EditProduct(EditProductDTO model);
        Task RemoveProduct(long id);
    }
}
