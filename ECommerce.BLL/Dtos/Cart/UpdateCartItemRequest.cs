using System;

namespace ECommerce.BLL.Dtos.Cart
{
    public record UpdateCartItemRequest(
        Guid ProductId,
        int Quantity
    );
}
