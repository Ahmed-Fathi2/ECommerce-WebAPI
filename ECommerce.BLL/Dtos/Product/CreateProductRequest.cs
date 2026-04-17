using ECommerce.DAL;

namespace ECommerce.BLL.Dtos.Product
{
    public record CreateProductRequest
    (

     string Name,
     string Description,
     decimal Price,
     int Count,
     string ImageUrl,
     Guid CategoryId
    );
}

