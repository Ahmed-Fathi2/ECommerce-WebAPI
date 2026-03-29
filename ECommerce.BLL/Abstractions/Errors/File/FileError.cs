using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Abstractions.Errors.File
{
    public static class FileError
    {
        public static readonly Error SchemaHostNotFound = new Error("Schema,HostNotFound", "Schema or Host NotFound", ErrorStatusCodes.BadRequest);
    }
}

