using Countries.Application.Interfaces;
using Countries.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Countries.Infrastructure;

public class RedisCache : ICache<Country>
{
    private readonly IDatabase _redisDb;
    private readonly TimeSpan _expiryTimeSpan = TimeSpan.FromSeconds(10);   // TODO: Should read from configuration

    public RedisCache(IConnectionMultiplexer connectionMultiplexer)
    {
        _redisDb = connectionMultiplexer.GetDatabase();
    }
    
    public async Task<(bool exists, Country? item)> TryGet(string key)
    {
        await _redisDb.PingAsync();
        
        if (!await _redisDb.KeyExistsAsync(key))
            return (false, null);

        var itemAsString = await _redisDb.StringGetAsync(key);
        var item = JsonConvert.DeserializeObject<Country>(itemAsString!);
        return (true, item);
    }

    public async Task Set(string key, Country item)
    {
        var itemAsString = JsonConvert.SerializeObject(item);
        var success = await _redisDb.StringSetAsync(key, itemAsString, _expiryTimeSpan);
        if (!success)
            throw new NotImplementedException();
    }
}