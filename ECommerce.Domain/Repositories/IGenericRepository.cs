using ECommerce.Domain.Repositories;
namespace ECommerce.Domain.Repositories
{
    public interface IGenericRepository<T> where T :class
    {

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(Guid id);
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}





