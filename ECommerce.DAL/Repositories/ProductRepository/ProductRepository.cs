using Microsoft.EntityFrameworkCore;
using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext):base(dbContext)
        {
            
        }

        public  async Task<IQueryable<Product>> GetAllProducts()
        {
            var products= _dbContext.Products.Include(x=>x.Category).AsQueryable();
            return products;
        }

        public async Task<Product?> GetProductByCategoryAsync(Guid id)
        {
            var product=  await _dbContext.Products
                    .Include(c=>c.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

            if(product is not null)
            {
                return product;
            }
            return null;
        }
    }
}

