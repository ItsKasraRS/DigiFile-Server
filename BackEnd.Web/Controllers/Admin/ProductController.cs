using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region constructor
        private IAdminProductService _adminProductService;
        private IAdminCategoryService _adminCategoryService;

        public ProductController(IAdminProductService adminProductService, IAdminCategoryService adminCategoryService)
        {
            _adminProductService = adminProductService;
            _adminCategoryService = adminCategoryService;
        }
        
        #endregion

        [HttpGet("list")]
        public async Task<IActionResult> GetProducts(int pageId = 1, string q ="")
        {
            return Ok(new { status="Success", data = await  _adminProductService.GetProductsForAdmin(pageId, q)});
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(new {status = "Success", data = await _adminCategoryService.GetCategories()});
        }

        [HttpGet("get-sub-categories/{categoryId}")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            return Ok(new { status = "Success", data = await _adminCategoryService.GetSubCategories(categoryId) });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct(AddProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation Error!" });
            }
            await _adminProductService.AddProductForAdmin(model);
            return Ok(new {status = "Success", message = "Product successfully added!"});
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _adminProductService.GetProductForAdmin(id);
            return Ok(new {status = "Success", data = product});
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditProduct(EditProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new {status = "Error", message = "Validation error!"});
            }

            await _adminProductService.EditProduct(model);
            return Ok(new {status = "Success", message = "product has been updated successfully!"});
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _adminProductService.RemoveProduct(id);
            return Ok(new {status = "Success", message = "product has been successfully removed!"});
        }
    }
}