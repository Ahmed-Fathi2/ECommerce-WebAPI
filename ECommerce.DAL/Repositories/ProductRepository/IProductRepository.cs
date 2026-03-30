using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.Domain;

namespace ECommerce.DAL.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<Product?> GetProductByCategoryAsync(Guid id);

        Task<IQueryable<Product>> GetAllProducts();
    }
}

