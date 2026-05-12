using ECommerce.Infrastructure.Data;
using ECommerce.Domain.Repositories;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICartItemRepository CartItemRepository { get; }
        public IOrderRepository OrderRepository { get; }

        public UnitOfWork(AppDbContext dbContext, IProductRepository productRepository, ICategoryRepository categoryRepository, ICartRepository cartRepository, ICartItemRepository cartItemRepository, IOrderRepository orderRepository)
        {
            _dbContext = dbContext;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            CartRepository = cartRepository;
            CartItemRepository = cartItemRepository;
            OrderRepository = orderRepository;
        }


        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        
    }
}








