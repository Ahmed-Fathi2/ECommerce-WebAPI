using ECommerce.DAL.Enums;

namespace ECommerce.DAL.Entities
{
    public class Order : IAuditableEntity
    {
        public Guid Id { get; set; }

        public OrderStatus OrderStatus { get; set; }= OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }



        public string ApplicationUserId { get; set; }=string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = default!;


        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
