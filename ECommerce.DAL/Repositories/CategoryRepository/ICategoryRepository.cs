using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.CategoryRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IQueryable<Category>> GetAllCategories();
    }
}
