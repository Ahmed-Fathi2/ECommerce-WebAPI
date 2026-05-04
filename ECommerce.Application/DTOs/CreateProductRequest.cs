using ECommerce.Application.DTOs;
namespace ECommerce.Application.DTOs
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





