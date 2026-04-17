using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Common.Pagination;

namespace ECommerce.BLL.Dtos.Product
{
    public record ProductRequestFilter:BaseRequestFilter
    {
        public Guid? CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}

