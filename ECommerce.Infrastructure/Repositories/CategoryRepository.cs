using ECommerce.Infrastructure.Data;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Repositories
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







