using ECommerce.Application.Common.Errors;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Errors
{
    public static class ProductError
    {
        public static readonly Error ProductNotFound =
            new Error("ProductNotFound", "The specified product was not found.", ErrorStatusCodes.NotFound);
    }
}





