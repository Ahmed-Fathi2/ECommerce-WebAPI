using ECommerce.Application.Common.Errors;
using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Errors
{
    public static class FileError
    {
        public static readonly Error SchemaHostNotFound = new Error("Schema,HostNotFound", "Schema or Host NotFound", ErrorStatusCodes.BadRequest);
    }
}





