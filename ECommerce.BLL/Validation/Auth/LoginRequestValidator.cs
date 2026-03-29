using FluentValidation;
using ECommerce.BLL.Dtos.Auth;

namespace ECommerce.BLL.Validation.Auth
{
    public class LoginRequestValidator:AbstractValidator<LoginRequest>
    {

        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}

