using Mapster;
using ECommerce.BLL.Dtos.Cart;
using ECommerce.DAL.Entities;
using System.Linq;

namespace ECommerce.BLL.MappingConfiguration.Cart
{
    internal class CartConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CartItem, CartItemResponse>()
                .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : string.Empty)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice);

            config.NewConfig<ECommerce.DAL.Entities.Cart, CartResponse>()
                .Map(dest => dest.UserId, src => src.ApplicationUserId)
                .Map(dest => dest.Items, src => src.CartItems)
                .Map(dest => dest.TotalCartPrice, src => src.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice));
        }
    }
}
