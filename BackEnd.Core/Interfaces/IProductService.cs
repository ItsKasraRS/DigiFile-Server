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
    }

    public interface IAdminProductService
    {
        Task<AdminFilterProducts> GetProductsForAdmin(int pageId = 1, string filterTitle = "");
        Task AddProductForAdmin(AddProductDTO model, IFormFile sourceFile);
        Task<EditProductDTO> GetProductForAdmin(long id);
        Task EditProduct(EditProductDTO model);
        Task RemoveProduct(long id);
    }
}
