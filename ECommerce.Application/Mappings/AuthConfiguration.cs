using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using Mapster;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings
{
    public class AuthConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email);
        }
    }
}




