using ECommerce.DAL;

namespace ECommerce.BLL.Dtos.Product
{
    public record ProductDetailsResponse
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Count,
        Guid CategoryId,
        string CategoryName,
        string CategoryDescription,
        string? ImageUrl
        );
}

