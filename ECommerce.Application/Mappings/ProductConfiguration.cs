using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using Mapster;

namespace ECommerce.Application.Mappings
{
    internal class ProductConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Product, ProductsResponse>()
                .Map(dest => dest.CategoryId, src => src.Category.Id);

            //ProductsResponse
        }
    }
}






