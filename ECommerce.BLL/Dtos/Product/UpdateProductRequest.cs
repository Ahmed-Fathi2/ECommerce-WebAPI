using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Product
{
    public record UpdateProductRequest
    (
     string Name,
     string Description,
     decimal Price,
     int Count,
     Guid CategoryId,
     string? ImageUrl = null
    );
}

