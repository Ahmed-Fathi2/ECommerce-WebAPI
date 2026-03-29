using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Product
{
    public record UpdateProductRequest
    (
     string Name,
     string Description,
     int Price,
     int Count
    );
}

