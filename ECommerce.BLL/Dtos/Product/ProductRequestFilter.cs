using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.BLL.Abstractions.Common;

namespace ECommerce.BLL.Dtos.Product
{
    public record ProductRequestFilter:BaseRequestFilter
    {
        public int? CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}

