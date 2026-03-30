using Mapster;
using ECommerce.BLL.Dtos.Product;
using ECommerce.DAL;

namespace ECommerce.BLL.MappingConfiguration.Product
{
    internal class ProductConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ECommerce.Domain.Product, ProductsResponse>()
                .Map(dest => dest.CaterogyId, src => src.Category.Id);

            config.NewConfig<ECommerce.Domain.Product, ProductsResponse>()
                .Map(dest => dest.CaterogyId, src => src.Category.Id);

            //ProductsResponse
        }
    }
}

