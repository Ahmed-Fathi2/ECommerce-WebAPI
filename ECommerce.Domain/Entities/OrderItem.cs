namespace ECommerce.Domain.Entities
{
    public class OrderItem : IAuditableEntity
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }


        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; } // Price at the time of order, to handle price changes in the future

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public Product Product { get; set; } = default!;
        public Order Order { get; set; }=default!;

    }

}

