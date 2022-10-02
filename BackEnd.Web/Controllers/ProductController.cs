using System.Threading.Tasks;
using BackEnd.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region constructor

        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        [HttpGet("get-latest-products")]
        public async Task<IActionResult> GetLatestProducts()
        {
            return Ok(new { status = "Success", data = await _productService.GetLatestProducts() });
        }
    }
}
