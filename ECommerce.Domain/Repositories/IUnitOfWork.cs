using ECommerce.Domain.Repositories;

namespace ECommerce.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get;  }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICartItemRepository CartItemRepository { get; }
        public IOrderRepository OrderRepository { get; }
        Task SaveAsync();
    }
}






