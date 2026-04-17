using ECommerce.DAL.Repositories.ProductRepository;
using ECommerce.DAL.Repositories.CategoryRepository;
using ECommerce.DAL.Repositories.CartRepository;
using ECommerce.DAL.Repositories.OrderRepository;

namespace ECommerce.DAL.Repositories.UnitOfWork
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

