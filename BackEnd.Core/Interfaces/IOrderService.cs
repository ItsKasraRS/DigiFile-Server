using BackEnd.Core.DTOs.Order;
using BackEnd.Core.DTOs.User;
using BackEnd.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Core.Interfaces
{
    public interface IOrderService
    {
        #region ======= * Order * =======

        #region Cart

        Task<Order> CreateUserOrder(long userId);

        Task AddProductToOrder(long userId, long productId);

        Task<Order> GetUserOpenOrder(long userId);

        Task<bool> IsExistProductInOrder(long productId, long userId);

        Task<List<OrderBasketDetail>> GetUserCartDetails(long userId);

        #endregion

        Task<List<ShowUserOrdersDTO>> GetUserOrders(long userId);
        Task ChangeOrderStatus(string status, long userId);

        #endregion

        #region ======= * Order Details * =======

        #region Delete order detail

        Task DeleteOrderDetail(long userId, OrderDetail detail);

        #endregion

        #endregion
    }
}
