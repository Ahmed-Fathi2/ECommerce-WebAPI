using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Auth;

namespace ECommerce.BLL.Managers.Auth
{
    public interface IAuthManager
    {
        Task<Result> RegisterAsync(RegisterRequest RegisterRequest,string origin);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);

        Task<Result> AddRole(string roleName);

        Task<Result> ConfirmEmail(EmailConfirmationRequest emailConfirmationRequest);
    }
}




