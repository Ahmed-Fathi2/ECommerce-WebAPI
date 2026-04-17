using ECommerce.Common.Constants;
using ECommerce.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Common.Errors.File
{
    public static class FileError
    {
        public static readonly Error SchemaHostNotFound = new Error("Schema,HostNotFound", "Schema or Host NotFound", ErrorStatusCodes.BadRequest);
    }
}

