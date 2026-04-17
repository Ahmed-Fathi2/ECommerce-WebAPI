using ECommerce.Common.Constants;
using ECommerce.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Common.Errors.Product
{
    public static class ProductError
    {
        public static readonly Error ProductNotFound =
            new Error("ProductNotFound", "The specified product was not found.", ErrorStatusCodes.NotFound);
    }
}

