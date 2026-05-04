using ECommerce.Application.DTOs;
namespace ECommerce.Application.DTOs
{
    public record CreateCategoryRequest
    (
        string Name,
        string Description,
        string? ImageUrl
    );
}



