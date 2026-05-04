using ECommerce.Application.Common.Errors;
using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Repositories;
using ECommerce.Application.Common.ResultPattern;
using ECommerce.Domain.Entities;
using Mapster;
using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Contracts;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<ECommerce.Domain.Entities.Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null)
            {
                cart = new ECommerce.Domain.Entities.Cart { ApplicationUserId = userId };
                _unitOfWork.CartRepository.Add(cart);
                await _unitOfWork.SaveAsync();
            }
            return cart;
        }

        public async Task<Result<CartResponse>> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null)
            {
                return Result.Success(new CartResponse(Guid.Empty, userId, Enumerable.Empty<CartItemResponse>(), 0));
            }

            return Result.Success(cart.Adapt<CartResponse>());
        }

        public async Task<Result<CartResponse>> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result.Failure<CartResponse>(CartError.ProductNotFound);

            var cart = await GetOrCreateCartAsync(userId);
            
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = request.Adapt<CartItem>();
                cartItem.CartId = cart.Id;
                cartItem.UnitPrice = product.Price;
                cartItem.Product = product;
                cart.CartItems.Add(cartItem);
            }

            await _unitOfWork.SaveAsync();

            return Result.Success(cart.Adapt<CartResponse>());
        }

        public async Task<Result> UpdateCartItemAsync(string userId, UpdateCartItemRequest request)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null)
                return Result.Failure(CartError.CartNotFound);

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (item == null)
                return Result.Failure(CartError.ItemNotFound);

            item.Quantity = request.Quantity;
            await _unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> RemoveFromCartAsync(string userId, Guid productId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null)
                return Result.Failure(CartError.CartNotFound);

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null)
                return Result.Failure(CartError.ItemNotFound);

            cart.CartItems.Remove(item);
            await _unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> ClearCartAsync(string userId)
        {
            var cart =await _unitOfWork.CartRepository.GetUserCartAsync(userId);

            if (cart == null)
                return Result.Failure(CartError.CartNotFound);
  
            _unitOfWork.CartItemRepository.DeleteRange(cart.CartItems);
            await _unitOfWork.SaveAsync();

            return Result.Success();
        }
    }
}








