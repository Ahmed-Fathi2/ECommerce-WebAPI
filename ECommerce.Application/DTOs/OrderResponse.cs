using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ECommerce.Application.DTOs
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



