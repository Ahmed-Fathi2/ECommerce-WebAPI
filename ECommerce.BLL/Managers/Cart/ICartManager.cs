using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Cart;
using System;
using System.Threading.Tasks;

namespace ECommerce.BLL.Managers.Cart
{
    public interface ICartManager
    {
        Task<Result<CartResponse>> GetCartAsync(string userId);
        Task<Result<CartResponse>> AddToCartAsync(string userId, AddToCartRequest request);
        Task<Result> UpdateCartItemAsync(string userId, UpdateCartItemRequest request);
        Task<Result> RemoveFromCartAsync(string userId, Guid productId);
        Task<Result> ClearCartAsync(string userId);
    }
}
