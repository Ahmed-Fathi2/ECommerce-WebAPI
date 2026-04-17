using Mapster;
using ECommerce.BLL.Dtos.Auth;
using ECommerce.DAL.Entities;

namespace ECommerce.BLL.MappingConfiguration.Auth
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

