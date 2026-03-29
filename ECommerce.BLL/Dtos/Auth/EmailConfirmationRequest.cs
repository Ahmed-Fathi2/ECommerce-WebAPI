using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Auth
{
    public sealed record EmailConfirmationRequest
     (
        string UserId,
        string Code
     );
}

