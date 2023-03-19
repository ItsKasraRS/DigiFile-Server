
using BackEnd.Core.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Core.Interfaces
{
    public interface ICommentService
    {
        Task<List<ShowCommentsDTO>> GetProductComments(long id);

        Task AddCommentToProduct(AddCommentDTO model, long productId, long userId);

        //User Panel
        Task<List<UserCommentsDTO>> GetUserComments(long id);
    }
}
