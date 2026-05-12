using ECommerce.Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? cachedValue = await _distributedCache.GetStringAsync( key, cancellationToken);

        if (string.IsNullOrEmpty(cachedValue))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task SetAsync<T>( string key, T value, CancellationToken cancellationToken = default)
    {
        string serializedValue = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(key,serializedValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            },
            cancellationToken);
    }

    public async Task RemoveAsync( string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync( key, cancellationToken);
    }
}