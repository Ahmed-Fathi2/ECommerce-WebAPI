namespace ECommerce.DAL
{
    public class Product : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Count { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }


        public Category Category { get; set; } = default!;

    }
}

