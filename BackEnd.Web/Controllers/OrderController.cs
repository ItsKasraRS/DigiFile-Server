using BackEnd.Core.DTOs.Order;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Utilities.Security;
using eWAY.Rapid;
using eWAY.Rapid.Enums;
using eWAY.Rapid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region constructor

        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        #region Add product to order

        [HttpGet("add-order/{productId}")]
        public async Task<IActionResult> AddProductToOrder(long productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                await _orderService.AddProductToOrder(userId, productId);
                return Ok(new { status="Success", message = "Product added to cart!" });
            }

            return BadRequest(new { message = "Please sign in to your account" });
        }

        #endregion

        #region Check product exist in order

        [HttpGet("check-order/{productId}")]
        public async Task<IActionResult> CheckOrder(long productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (await _orderService.IsExistProductInOrder(productId, User.GetUserId()))
                {
                    return Ok(new { status = "IsExist" });
                }

                return Ok(new { status = "NotExist" });
            }
            else
            {
                return Ok(new { status = "NotIdentified" });
            }
        }

        #endregion

        #region Show Cart For User

        [HttpGet("user-cart")]
        public async Task<IActionResult> ShowCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { status = "Success", data = await _orderService.GetUserCartDetails(User.GetUserId()) });
            }

            return Unauthorized();
        }

        #endregion

        #region Remove item from order

        [HttpGet("remove-item/{id}")]
        public async Task<IActionResult> RemoveItemFromOrder([FromRoute] long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var order = await _orderService.GetUserOpenOrder(User.GetUserId());
                var detail = order.OrderDetails.SingleOrDefault(d => d.Id == id);
                if (detail == null)
                {
                    return BadRequest();
                }

                await _orderService.DeleteOrderDetail(User.GetUserId(), detail);

                return Ok(new { status = "Success", data = await _orderService.GetUserCartDetails(User.GetUserId()) });
            }

            return Unauthorized();
        }

        #endregion

        [HttpGet("get-cart-count")]
        public async Task<IActionResult> GetCartCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { status = "Success", data = _orderService.GetUserCartDetails(User.GetUserId()).Result.Count() });
            }

            return Unauthorized();
        }

        [HttpPost("checkout-payment")]
        public async Task<IActionResult> CheckoutPayment(OrderCheckoutDTO model)
        {
            string apiKey = "A1001CdPPpF5MIhn+IF4nCQghQCOdOpBBtGgTcndFEh3VaaGmdjwo0OtSZktCzUsDPZsoF";
            string password = "e0Y0fvNF";
            string rapidEndpoint = "Sandbox";
            IRapidClient ewayClient = RapidClientFactory.NewRapidClient(apiKey, password, rapidEndpoint);

            int price = int.Parse(model.Price.ToString().Replace(".", ""));

            Transaction transaction = new Transaction()
            {
                PaymentDetails = new PaymentDetails()
                {
                    TotalAmount = price,
                    
                },
                RedirectURL = "http://localhost:4200/payment-result",
                CancelURL = "http://localhost:4200/payment-result?AccessCode=Cancelled",
                TransactionType = TransactionTypes.Purchase
            };
            CreateTransactionResponse response = ewayClient.Create(PaymentMethod.ResponsiveShared, transaction);


            if (response.Errors != null)
            {
                foreach (string errorCode in response.Errors)
                {
                    return BadRequest(RapidClientFactory.UserDisplayMessage(errorCode, "EN"));
                }
            }


            return Ok(new { result = response });

        }

        [HttpGet("payment-result")]
        public async Task<IActionResult> PaymentResult(string AccessCode)
        {
            if(AccessCode == "Cancelled")
            {
                return Ok(new { status = "Cancelled" });
            }

            string apiKey = "A1001CdPPpF5MIhn+IF4nCQghQCOdOpBBtGgTcndFEh3VaaGmdjwo0OtSZktCzUsDPZsoF";
            string password = "e0Y0fvNF";
            string rapidEndpoint = "Sandbox";
            IRapidClient ewayClient = RapidClientFactory.NewRapidClient(apiKey, password, rapidEndpoint);

            QueryTransactionResponse response = ewayClient.QueryTransaction(AccessCode);

            if(response.Errors != null)
            {
                await _orderService.ChangeOrderStatus("Failed", User.GetUserId());
            }

            await _orderService.ChangeOrderStatus("Paid", User.GetUserId());

            return Ok(response);
        }
    }
}
