using StackExchange.Redis;

namespace RedisExampleCache
{
    public interface IRedisService
    {
        Task<bool> CheckKeyExistsAsync(string key);
        Task AddItemToHashSetAsync(string key, int id, string value);
        Task<HashEntry[]?> GetHashSetAsync(string key);
        Task<RedisValue> GetHashSetItemAsync(string key, int id);
    }
}
