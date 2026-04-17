using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ECommerce.BLL.Abstractions.Auth;
using ECommerce.DAL.Entities;

namespace ECommerce.BLL.Managers.Auth
{
    // Empty implementation of IJwtProvider
    public class JwtProvider : IJwtProvider
    {
        public JwtProvider()
        {
        }

        public (string token, int expireIn) GenerateToken(ApplicationUser user , IEnumerable<string> roles )
        {
            var claims = new List<Claim>
            {

                 new(JwtRegisteredClaimNames.Sub,user.Id),
                 new (JwtRegisteredClaimNames.GivenName,user.FirstName),
                 new (JwtRegisteredClaimNames.FamilyName,user.LastName),
                 new (JwtRegisteredClaimNames.Email,user.Email!),
                 new (JwtRegisteredClaimNames.Jti,Guid.CreateVersion7().ToString()),
               //new(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),

          
                //لToDo---->>>  Add User_Permission
            };

            // Add User_Roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //Use option Pattern
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7K+2mXqP9vRnLwA3sYdF8hJcT5bNgUeZ1oQpWkViCxMj6rHyDs4OtBIlEuAfG0n+mXqP9vRnLwB4="));

            var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            int expireIn = 30;
            var token = new JwtSecurityToken(
                issuer: "iti",
                audience: "iti_students",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireIn),
                signingCredentials: singingCredentials
            );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expireIn * 60);

        }
    }
}

