using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
    }
}





