using ECommerce.Application.DTOs;
using ECommerce.Application.Common.ResultPattern;
using System;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface ICartService
    {
        Task<Result<CartResponse>> GetCartAsync(string userId);
        Task<Result<CartResponse>> AddToCartAsync(string userId, AddToCartRequest request);
        Task<Result> UpdateCartItemAsync(string userId, UpdateCartItemRequest request);
        Task<Result> RemoveFromCartAsync(string userId, Guid productId);
        Task<Result> ClearCartAsync(string userId);
    }
}






