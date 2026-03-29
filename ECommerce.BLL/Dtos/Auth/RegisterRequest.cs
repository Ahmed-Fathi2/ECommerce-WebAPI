using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Dtos.Auth
{
    public sealed  record RegisterRequest( string FirstName,string LastName,string Email, string Password);
    
}

