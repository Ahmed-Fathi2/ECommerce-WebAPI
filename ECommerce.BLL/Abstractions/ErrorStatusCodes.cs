using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Abstractions
{
    public static class ErrorStatusCodes
    {
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int Conflict = 409;
    }
}

