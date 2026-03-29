using ECommerce.DAL.Repositories.ProductRepository;

namespace ECommerce.DAL.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get;  }
        Task SaveAsync();
    }
}

