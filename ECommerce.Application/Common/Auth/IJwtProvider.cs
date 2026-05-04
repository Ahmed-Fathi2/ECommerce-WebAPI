using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Auth
{
    // Empty marker interface for JWT provider
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);

    }
}



