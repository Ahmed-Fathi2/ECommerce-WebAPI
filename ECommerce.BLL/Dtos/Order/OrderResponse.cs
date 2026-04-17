using ECommerce.DAL.Enums;
using System;
using System.Collections.Generic;

namespace ECommerce.BLL.Dtos.Order
{
    public record OrderItemResponse(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
    );

    public record OrderResponse(
        Guid Id,
        string OrderStatus,
        decimal TotalAmount,
        DateTime CreatedAt,
        IEnumerable<OrderItemResponse> Items
    );
}
