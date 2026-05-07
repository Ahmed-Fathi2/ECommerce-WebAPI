using ECommerce.Application.DTOs;
namespace ECommerce.Application.DTOs
{
    public class ProductDetailsResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public string? ImageUrl { get; set; }
    }
}





