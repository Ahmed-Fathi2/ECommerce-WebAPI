using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.BLL.Managers.Order
{
    public interface IOrderManager
    {
        Task<Result<string>> PlaceOrderAsync(string origin, string userId);
        Task<Result<IEnumerable<OrderResponse>>> GetUserOrdersAsync(string userId);
        Task<Result<OrderResponse>> GetOrderDetailsAsync(Guid orderId, string userId);
    }
}
