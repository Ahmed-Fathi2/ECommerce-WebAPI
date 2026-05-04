using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public sealed record EmailConfirmationRequest
     (
        string UserId,
        string Code
     );
}




