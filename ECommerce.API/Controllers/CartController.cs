using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Settings;
using ECommerce.API.Helpers;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Contracts;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireCustomer")]
    //[Authorize(Roles = "Admin,Customer")]
    public class CartController(ICartService CartService) : ControllerBase
    {
        private readonly ICartService _cartManager = CartService;

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






