using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IQueryable<Category>> GetAllCategories();
    }
}





