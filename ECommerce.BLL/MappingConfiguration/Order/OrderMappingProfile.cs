using ECommerce.BLL.Dtos.Order;
using Mapster;

namespace ECommerce.BLL.MappingConfiguration.Order
{
    public class OrderMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ECommerce.DAL.Entities.OrderItem, OrderItemResponse>()
                .Map(dest => dest.ProductName, src => src.Product.Name)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice);

            config.NewConfig<ECommerce.DAL.Entities.Order, OrderResponse>()
                .Map(dest => dest.Items, src => src.OrderItems);
        }
    }
}
