using System.Threading.Tasks;
using BackEnd.Core.DTOs.Product;
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

        [HttpGet("get-popular-products")]
        public async Task<IActionResult> GetPopularProducts()
        {
            return Ok(new { status = "Success", data = await _productService.GetPopularProducts() });
        }

        [HttpGet("filter-products")]
        public async Task<IActionResult> FilterProducts([FromQuery] FilterProductsDTO filter)
        {
            var product = await _productService.FilterProducts(filter);
            return Ok(new { status = "Success", data = product });
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> ProductDetails(long id)
        {
            var product = _productService.GetProductDetails(id);
            if (product == null) return BadRequest(new { status = "Error", message = "Product not found!" });


            return Ok(new { status = "Success", data = product.Result });
        }

        [HttpGet("get-gallery/{id}")]
        public async Task<IActionResult> GetProductGallery(long id)
        {
            return Ok(new { status = "Success", data = await _productService.GetProductGallery(id) });
        }
    }
}
