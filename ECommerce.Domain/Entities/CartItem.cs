using ECommerce.Domain;

namespace ECommerce.Domain
{
    public class CartItem : IAuditableEntity
    {

        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public Product Product { get; set; } = default!;
        public Cart Cart { get; set; } = default!;
    }
}
