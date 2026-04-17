using ECommerce.Common.Errors.Cart;
using ECommerce.Common.Errors.Order;
using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Order;
using ECommerce.DAL.Repositories.UnitOfWork;
using ECommerce.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.BLL.Managers.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> PlaceOrderAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return Result.Failure<Guid>(OrderError.CartIsEmpty);

            var order = new ECommerce.DAL.Entities.Order
            {
                ApplicationUserId = userId,
                OrderStatus = ECommerce.DAL.Enums.OrderStatus.Pending,
                TotalAmount = cart.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };

            _unitOfWork.OrderRepository.Add(order);


            // Clear the cart
            _unitOfWork.CartItemRepository.DeleteRange(cart.CartItems);

            await _unitOfWork.SaveAsync();

            return Result.Success(order.Id);
        }

        public async Task<Result<IEnumerable<OrderResponse>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetUserOrdersAsync(userId);
            return Result.Success(orders.Adapt<IEnumerable<OrderResponse>>());
        }

        public async Task<Result<OrderResponse>> GetOrderDetailsAsync(Guid orderId, string userId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderDetailsAsync(orderId, userId);
            if (order == null)
                return Result.Failure<OrderResponse>(OrderError.OrderNotFound);

            return Result.Success(order.Adapt<OrderResponse>());
        }
    }
}
