using ECommerce.DAL.Repositories.GenericRepository;

namespace ECommerce.DAL.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<Product?> GetProductByCategoryAsync(int id);

        Task<IQueryable<Product>> GetAllProducts();
    }
}

