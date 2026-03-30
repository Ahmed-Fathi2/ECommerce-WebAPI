using ECommerce.BLL.Abstractions.Constants;
using ECommerce.BLL.Abstractions.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Abstractions.Errors.User
{
    public class UserError
    {
        public static readonly Error EmailAlreadyExists = new ("EmailAlreadyExist", "Email already exist", ErrorStatusCodes.Conflict);
        public static readonly Error InvalidEmailOrPassword = new ("InvalidEmailOrPassword", "Invalid email or password", ErrorStatusCodes.Unauthorized);
        public static readonly Error EmailNotConfirmed = new ("EmailNotConfirmed", "Email not confirmed", ErrorStatusCodes.Unauthorized);
        public static readonly Error UserLockedOut = new ("UserLockedOut", "User is locked out", ErrorStatusCodes.Unauthorized);
        public static readonly Error UserNotFound=new ("UserNotFound","User not found",ErrorStatusCodes.NotFound);
        public static readonly Error InvalidCode=new ("InvalidCode","Invalid code",ErrorStatusCodes.BadRequest);
    }
}

