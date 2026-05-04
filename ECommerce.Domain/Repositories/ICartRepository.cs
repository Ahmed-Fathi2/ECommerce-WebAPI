using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetUserCartAsync(string userId);
    }
}





