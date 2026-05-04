using ECommerce.Application.DTOs;
using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Contracts
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterRequest RegisterRequest,string origin);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);

        Task<Result> AddRole(string roleName);

        Task<Result> ConfirmEmail(EmailConfirmationRequest emailConfirmationRequest);
    }
}










