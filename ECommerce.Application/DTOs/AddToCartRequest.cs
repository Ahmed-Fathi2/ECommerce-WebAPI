using ECommerce.Application.DTOs;
using System;

namespace ECommerce.Application.DTOs
{
    public record AddToCartRequest(
        Guid ProductId,
        int Quantity
    );
}



