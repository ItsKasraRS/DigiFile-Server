using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Utilities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackEnd.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region constructor

        private ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #endregion


        [HttpGet("get-product-comments/{id}")]
        public async Task<IActionResult> GetProductComments(long id)
        {
            var comments = await _commentService.GetProductComments(id);

            return Ok(new { status = "Success", data = comments });
        }

        [HttpPost("add-comment/{productId}")]
        public async Task<IActionResult> AddCommentToProduct(AddCommentDTO model, long productId)
        {
            if (!ModelState.IsValid)
            return BadRequest("Please write your comment first");


            await _commentService.AddCommentToProduct(model, productId, User.GetUserId());
            return Ok(new { status = "Success", message = "Your comment has been submitted and is awaiting approval" });
        }
    }
}
