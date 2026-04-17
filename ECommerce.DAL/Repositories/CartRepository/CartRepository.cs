using ECommerce.DAL;
using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Repositories.CartRepository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cart?> GetUserCartAsync(string userId)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
        }
    }
}
