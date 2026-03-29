using ECommerce.DAL.Data.Entities;

namespace ECommerce.BLL.Abstractions.Auth
{
    // Empty marker interface for JWT provider
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);

    }
}

