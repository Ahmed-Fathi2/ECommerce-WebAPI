using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.CartRepository
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
    }
}
