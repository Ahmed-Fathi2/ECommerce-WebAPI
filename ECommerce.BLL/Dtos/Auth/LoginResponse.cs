using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Auth
{
    public sealed record LoginResponse
    (
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string Token,
        int ExpiresIn
        );
    
}

