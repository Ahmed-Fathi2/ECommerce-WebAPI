using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public record ProductsResponse
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Count,
        Guid CategoryId,
        string CategoryName,
        string? ImageUrl
    );
}




