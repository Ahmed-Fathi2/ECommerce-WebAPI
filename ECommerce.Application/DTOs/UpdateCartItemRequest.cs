using ECommerce.Application.DTOs;
using System;

namespace ECommerce.Application.DTOs
{
    public record UpdateCartItemRequest(
        Guid ProductId,
        int Quantity
    );
}



