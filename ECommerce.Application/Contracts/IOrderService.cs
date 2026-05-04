using ECommerce.Application.DTOs;
using ECommerce.Application.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<string>> PlaceOrderAsync(string origin, string userId);
        Task<Result<IEnumerable<OrderResponse>>> GetUserOrdersAsync(string userId);
        Task<Result<OrderResponse>> GetOrderDetailsAsync(Guid orderId, string userId);
    }
}






