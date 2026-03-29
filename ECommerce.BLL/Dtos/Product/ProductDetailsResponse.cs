using ECommerce.DAL;

namespace ECommerce.BLL.Dtos.Product
{
    public record ProductDetailsResponse
    (
        int Id,
        string Name,
        string Description,
        int Price,
        int Count,
        string CategoryName,
        string CategoryDescription
        );
}

