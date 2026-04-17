using ECommerce.Common.ResultPattern;

namespace ECommerce.Common.Errors.Order
{
    public static class OrderError
    {
        public static readonly Error OrderNotFound =
            new("OrderNotFound", "Order could not be found.", 404);
            
        public static readonly Error CartIsEmpty =
            new("CartIsEmpty", "Cannot place an order because the shopping cart is empty.", 400);
    }
}
