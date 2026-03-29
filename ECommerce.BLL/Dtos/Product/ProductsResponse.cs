using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Product
{
    public record ProductsResponse
    (
        int Id,
        string Name,
        string Description,
        int Price,
        int Count,
        int CaterogyId,
        string CategoryName
    );
}

