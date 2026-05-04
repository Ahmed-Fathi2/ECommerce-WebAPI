using ECommerce.Application.Common.Errors;
using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Common.Errors
{
    public static class CartError
    {
        public static readonly Error CartNotFound = new Error("Cart.NotFound", "The cart was not found.", 404);
        public static readonly Error ItemNotFound = new Error("Cart.ItemNotFound", "The item is not in the cart.", 404);
        public static readonly Error ProductNotFound = new Error("Cart.ProductNotFound", "The product does not exist.", 404);
    }
}




