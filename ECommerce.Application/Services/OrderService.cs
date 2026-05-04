using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Repositories;
using ECommerce.Application.Common.ResultPattern;
using ECommerce.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Common.Errors;
using ECommerce.Application.Contracts;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Result<string>> PlaceOrderAsync(string origin, string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return Result.Failure<string>(OrderError.CartIsEmpty);

            var order = new ECommerce.Domain.Entities.Order
            {
                ApplicationUserId = userId,
                OrderStatus = ECommerce.Domain.Enums.OrderStatus.Pending,
                TotalAmount = cart.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };

            //var order = cart.Adapt<ECommerce.Domain.Entities.Order>();
            //order.ApplicationUserId = userId;

            _unitOfWork.OrderRepository.Add(order);
            await _unitOfWork.SaveAsync();

            var result = await _paymentService.InitiatePaymentAsync(origin, order.Id);

            if (!result.IsSuccess)
            {
                _unitOfWork.OrderRepository.Delete(order);
                await _unitOfWork.SaveAsync();
                return Result.Failure<string>(PaymentErrors.PaymentFailed);
            }

            order.SessionId = result.Value;

            await _unitOfWork.SaveAsync();

            return Result.Success(result.Value);
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









