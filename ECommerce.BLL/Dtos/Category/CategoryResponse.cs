using System;

namespace ECommerce.BLL.Dtos.Category
{
    public record CategoryResponse
    (
        Guid Id,
        string Name,
        string Description,
        string? ImageUrl
    );
}
