using Mapster;
using ECommerce.BLL.Dtos.Product;
using ECommerce.DAL;

namespace ECommerce.BLL.MappingConfiguration.Product
{
    internal class ProductConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ECommerce.DAL.Entities.Product, ProductsResponse>()
                .Map(dest => dest.CategoryId, src => src.Category.Id);

            //ProductsResponse
        }
    }
}

