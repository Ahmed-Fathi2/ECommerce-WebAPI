using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Settings;
using ECommerce.API.Helpers;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Contracts;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCustomer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderManager;

        public OrdersController(IOrderService OrderService)
        {
            _orderManager = OrderService;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            var userId = User.GetUserId();
            //var origin = $"{Request.Scheme}://{Request.Host}";
            var origin = "https://brethren-kilobyte-deflected.ngrok-free.dev";



            var result = await _orderManager.PlaceOrderAsync(origin,userId);

            return (result.IsSuccess) ? Ok(new { SessionUrl = result.Value }) : result.ToProblem();

            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersHistory()
        {
            var userId = User.GetUserId();
            var result = await _orderManager.GetUserOrdersAsync(userId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderDetails([FromRoute] Guid id)
        {
            var userId = User.GetUserId();
            var result = await _orderManager.GetOrderDetailsAsync(id, userId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}







