using System;

namespace ECommerce.BLL.Dtos.Cart
{
    public record AddToCartRequest(
        Guid ProductId,
        int Quantity
    );
}
