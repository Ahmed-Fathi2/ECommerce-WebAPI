using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.CartRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetUserCartAsync(string userId);
    }
}
