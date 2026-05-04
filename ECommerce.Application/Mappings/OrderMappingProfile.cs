using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using Mapster;

namespace ECommerce.Application.Mappings
{
    public class OrderMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.OrderItem, OrderItemResponse>()
                .Map(dest => dest.ProductName, src => src.Product.Name)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice);

            config.NewConfig<Domain.Entities.Order, OrderResponse>()
                .Map(dest => dest.Items, src => src.OrderItems);
        }
    }
}




