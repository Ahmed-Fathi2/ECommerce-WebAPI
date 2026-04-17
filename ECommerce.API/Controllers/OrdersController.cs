using ECommerce.API.Extensions;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Order;
using ECommerce.BLL.Managers.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCustomer")]
    //[Authorize(Roles = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            var userId = User.GetUserId();
            var result = await _orderManager.PlaceOrderAsync(userId);

            return (result.IsSuccess) ? Ok(new { OrderId = result.Value }) : result.ToProblem();
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
