using ECommerce.DAL;

namespace ECommerce.BLL.Dtos.Product
{
    public record CreateProductRequest
    (

     string Name,
     string Description,
     int Price,
     int Count,
     string ImageUrl,
     int CategoryId
    );
}

