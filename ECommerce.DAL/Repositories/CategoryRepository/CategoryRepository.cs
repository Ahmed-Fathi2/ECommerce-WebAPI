using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<Category>> GetAllCategories()
        {
            return _dbContext.Categories.AsQueryable();
        }
    }
}
