using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Entities
{
    public class Product : IAuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

    }
}

