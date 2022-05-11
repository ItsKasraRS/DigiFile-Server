using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackEnd.Web.Controllers.Admin
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetProducts(int pageId = 1, string q = "")
        {
            return Ok(new { status = "Success", data = await _adminProductService.GetProductsForAdmin(pageId, q) });
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(new { status = "Success", data = await _adminCategoryService.GetCategories() });
        }

        [HttpGet("get-sub-categories/{categoryId}")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            return Ok(new { status = "Success", data = await _adminCategoryService.GetSubCategories(categoryId) });
        }

        [HttpPost("add"), DisableRequestSizeLimit]
        public async Task<IActionResult> AddProduct([FromForm]AddProductDTO model)
        {
            var files = Request.Form.Files[0];
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation Error!" });
            }

            var fileExtension = Path.GetExtension(files.FileName);
            if (fileExtension != ".zip" && fileExtension != ".rar")
            {
                return Ok(new { status = "Error", message = "File format must be .zip or .rar" });
            }
            await _adminProductService.AddProductForAdmin(model, files);
            return Ok(new { status = "Success", message = "Product successfully added!" });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _adminProductService.GetProductForAdmin(id);
            return Ok(new { status = "Success", data = product });
        }

        [HttpPost("edit"), DisableRequestSizeLimit]
        public async Task<IActionResult> EditProduct([FromForm]EditProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation error!" });
            }
            var fileExtension = Path.GetExtension(model.SelectedSourceFile.FileName);
            if (fileExtension != ".zip" && fileExtension != ".rar")
            {
                return Ok(new { status = "Error", message = "File format must be .zip or .rar" });
            }
            await _adminProductService.EditProduct(model);
            return Ok(new { status = "Success", message = "product has been updated successfully!" });
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _adminProductService.RemoveProduct(id);
            return Ok(new { status = "Success", message = "product has been successfully removed!" });
        }
    }
}