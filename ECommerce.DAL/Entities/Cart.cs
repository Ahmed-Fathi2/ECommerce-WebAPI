namespace ECommerce.DAL.Entities
{
    public class Cart : IAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get ; set; }
        public DateTime? UpdatedAt { get ; set; }


        public string ApplicationUserId { get; set; }= string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

    }
}
