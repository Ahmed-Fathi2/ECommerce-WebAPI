using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key , CancellationToken cancellationToken = default);

        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
