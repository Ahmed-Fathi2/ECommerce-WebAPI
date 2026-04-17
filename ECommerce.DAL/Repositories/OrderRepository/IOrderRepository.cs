using ECommerce.DAL.Repositories.GenericRepository;
using ECommerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.DAL.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderDetailsAsync(Guid orderId, string userId);
    }
}
