using ECommerce.Application.DTOs;
namespace ECommerce.Application.DTOs
{
    public record CartItemResponse(
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
    );

    public record CartResponse(
        Guid Id,
        string UserId,
        IEnumerable<CartItemResponse> Items,
        decimal TotalCartPrice
    );
}



