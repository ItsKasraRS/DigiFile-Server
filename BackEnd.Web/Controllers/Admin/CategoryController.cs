using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Category;
using BackEnd.Core.Interfaces;
using BackEnd.DataLayer.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region constructor

        private IAdminCategoryService _categoryService;

        public CategoryController(IAdminCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet("list")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            var subCategories = await _categoryService.ListAllSubCategories();
            return Ok(new {status = "Success", data = new {categories = categories, subCategories = subCategories}});
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCategory(AddCategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new {status = "Error", message = "Validation Error!"});
            }

            var category = await _categoryService.AddCategory(model);

            return Ok(new {status = "Success", message = "category has been added successfully!", data=category});
        }

        [HttpPost("addSub")]
        public async Task<IActionResult> AddSubCategory(AddSubCategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation Error!" });
            }

            var category = await _categoryService.AddSubCategory(model);

            return Ok(new { status = "Success", message = "category has been added successfully!", data = category });
        }

        [HttpPost("get/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            return Ok(new { status = "Success", data = await _categoryService.GetCategory(id)});
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation Error!" });
            }

            await _categoryService.EditCategory(model);

            return Ok(new { status = "Success", message = "category has been updated successfully!" });
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            await _categoryService.RemoveCategory(id);
            return Ok(new {status = "Success", message = "category has been removed successfully!"});
        }
    }
}