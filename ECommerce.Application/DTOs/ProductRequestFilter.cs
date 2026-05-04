using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common.Pagination;

namespace ECommerce.Application.DTOs
{
    public record ProductRequestFilter:BaseRequestFilter
    {
        public Guid? CategoryId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}




