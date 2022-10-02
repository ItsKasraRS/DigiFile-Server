using System.Threading.Tasks;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region constructor

        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(new { status = "Success", data = await _categoryService.GetCategoriesForHeader() });
        }
    }
}
