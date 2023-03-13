using BackEnd.Core.DTOs.Order;
using BackEnd.Core.DTOs.User;
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
    public class OrderService : IOrderService
    {
        #region constructor

        private SiteContext _context;
        private IProductService _productService;
        private IUserService _userService;

        public OrderService(SiteContext context, IProductService productService, IUserService userService)
        {
            _context = context;
            _productService = productService;
            _userService = userService;
        }

        #endregion

        #region ======= * Order * =======

        #region Cart

        #region Create User Order

        public async Task<Order> CreateUserOrder(long userId)
        {
            var order = new Order()
            {
                IsFinally = "Waiting",
                CreateDate = DateTime.Now,
                UserId = userId
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        #endregion

        #region Get User Open Order

        public async Task<Order> GetUserOpenOrder(long userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(o => o.Product).SingleOrDefaultAsync(o => o.IsFinally == "Waiting" && o.UserId == userId && !o.IsDelete);
            if (order == null)
            {
                order = await CreateUserOrder(userId);
            }

            return order;
        }



        #endregion

        #region Is Exist Product In Order

        public async Task<bool> IsExistProductInOrder(long productId, long userId)
        {
            var order = await GetUserOpenOrder(userId);
            var orderDetail = await _context.OrderDetails.Include(od => od.Order).SingleOrDefaultAsync(od => od.OrderId == order.Id && od.ProductId == productId);
            if (orderDetail != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Get User Cart Details

        public async Task<List<OrderBasketDetail>> GetUserCartDetails(long userId)
        {
            var order = await GetUserOpenOrder(userId);
            if (order == null)
            {
                return null;
            }

            return order.OrderDetails.Select(od => new OrderBasketDetail()
            {
                Title = od.Product.Title,
                Image = od.Product.Image,
                Price = od.Price,
                Id = od.Id
            }).ToList();
        }

        #endregion

        #endregion

        #endregion

        #region Get User Orders

        public async Task<List<ShowUserOrdersDTO>> GetUserOrders(long userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId && o.IsFinally != "Waiting").Select(o => new ShowUserOrdersDTO
            {
                CreateDate = o.CreateDate,
                IsPaid = o.IsFinally,
                Sum = o.OrderSum,
                OrderId = o.Id,
                Products = o.OrderDetails.Select(od => od.Product).Select(p => new OrderProductsDTO
                {
                    Id = p.Id,
                    Title = p.Title
                }).ToList()
            }).ToListAsync();
        }

        #endregion

        #region ======= * Order Details * =======

        #region Cart

        public async Task AddProductToOrder(long userId, long productId)
        {
            var user = await _userService.GetUserById(userId);
            var product = await _productService.GetProductForUserOrder(productId);

            if (user != null && product != null)
            {
                var order = await GetUserOpenOrder(userId);
                var orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Price = product.Price,
                };
                order.OrderSum = (order.OrderSum + orderDetail.Price);
                await _context.OrderDetails.AddAsync(orderDetail);
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

            }
        }

        #endregion

        #region Delete Order detail

        public async Task DeleteOrderDetail(long userId, OrderDetail detail)
        {
            var order = await GetUserOpenOrder(userId);
            order.OrderSum = (order.OrderSum - detail.Price);
            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();
        }

        #endregion

        #endregion

    }
}
