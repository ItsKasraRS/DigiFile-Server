using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using BackEnd.DataLayer.Context;
using BackEnd.DataLayer.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Core.Services
{
    public class CommentService : ICommentService
    {
        #region constructor

        private SiteContext _context;

        public CommentService(SiteContext context)
        {
            _context = context;
        }

        #endregion


        public async Task<List<ShowCommentsDTO>> GetProductComments(long id)
        {
            var comments = await _context.Comment.Include(c=>c.User).Where(c => c.ProductId == id && !c.IsDelete).Select(c=> new ShowCommentsDTO
            {
                Id = c.Id,
                Text = c.Text,
                UserAvatar = c.User.ImageAvatar,
                UserName = c.User.Username
            }).ToListAsync();

            return comments;
        }


        public async Task AddCommentToProduct(AddCommentDTO model, long productId, long userId)
        {
            var comment = new Comment
            {
                ProductId = productId,
                CreateDate = DateTime.Now,
                Status = "Waiting",
                Text = model.Text,
                UserId = userId,
                IsDelete = false
            };

            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        // User Panel
        public async Task<List<UserCommentsDTO>> GetUserComments(long id)
        {
            var comments = await _context.Comment.Include(c => c.Product).Where(c => c.UserId == id).Select(c => new UserCommentsDTO
            {
                Text = c.Text,
                CreateDate = c.CreateDate,
                ProductTitle = c.Product.Title,
                ProductId = c.ProductId,
                Status = c.Status
            }).ToListAsync();

            return comments;
        }
    }
}
