using ECommerce.API.Extensions;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Cart;
using ECommerce.BLL.Managers.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCustomer")]
    //[Authorize(Roles = "Admin,Customer")]
    public class CartController(ICartManager cartManager) : ControllerBase
    {
        private readonly ICartManager _cartManager = cartManager;

        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCart()
        {
            var userId = User.GetUserId();
            var result = await _cartManager.GetCartAsync(userId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        public async Task<ActionResult<CartResponse>> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.GetUserId();
            var result = await _cartManager.AddToCartAsync(userId, request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            var userId = User.GetUserId();
            var result = await _cartManager.UpdateCartItemAsync(userId, request);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveFromCart([FromRoute] Guid productId)
        {
            var userId = User.GetUserId();
            var result = await _cartManager.RemoveFromCartAsync(userId, productId);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
