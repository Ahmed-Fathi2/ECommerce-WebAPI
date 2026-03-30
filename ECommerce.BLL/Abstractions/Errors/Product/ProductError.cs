using ECommerce.BLL.Abstractions.Constants;
using ECommerce.BLL.Abstractions.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Abstractions.Errors.Product
{
    public static class ProductError
    {
        public static readonly Error ProductNotFound =
            new Error("ProductNotFound", "The specified product was not found.", ErrorStatusCodes.NotFound);
    }
}

