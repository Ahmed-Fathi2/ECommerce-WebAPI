using ECommerce.DAL;
using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.CartRepository
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
    }
}
