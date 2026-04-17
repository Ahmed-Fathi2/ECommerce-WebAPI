using ECommerce.Common.Errors.Cart;
using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Cart;
using ECommerce.DAL.Repositories.UnitOfWork;
using ECommerce.DAL.Entities;
using Mapster;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BLL.Managers.Cart
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task<ECommerce.DAL.Entities.Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null)
            {
                cart = new ECommerce.DAL.Entities.Cart { ApplicationUserId = userId };
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
