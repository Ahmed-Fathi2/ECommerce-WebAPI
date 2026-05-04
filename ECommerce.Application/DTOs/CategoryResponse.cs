using ECommerce.Application.DTOs;
using System;

namespace ECommerce.Application.DTOs
{
    public record CategoryResponse
    (
        Guid Id,
        string Name,
        string Description,
        string? ImageUrl
    );
}



