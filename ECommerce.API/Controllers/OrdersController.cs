using ECommerce.API.Helpers;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Order;
using ECommerce.BLL.Managers.Order;
using ECommerce.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "RequireCustomer")]
    //[Authorize(Roles = DefaultRole.Customer)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpPost]
        [Authorize(Roles=DefaultRole.Customer)]
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
