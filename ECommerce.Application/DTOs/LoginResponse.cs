using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
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




