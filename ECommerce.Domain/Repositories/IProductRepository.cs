using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<Product?> GetProductByCategoryAsync(Guid id);

        Task<IQueryable<Product>> GetAllProducts();
    }
}






