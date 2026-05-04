using ECommerce.Application.Contracts;
using ECommerce.Application.Common.Settings;
using System.Security.Claims;

namespace ECommerce.API.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}


