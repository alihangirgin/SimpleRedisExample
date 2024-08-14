using StackExchange.Redis;

namespace RedisExampleCache
{
    public class RedisService : IRedisService
    {
        private static IDatabase _db;

        public RedisService(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _db = redis.GetDatabase();
        }

        public async Task<bool> CheckKeyExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task AddItemToHashSetAsync(string key, int id, string value)
        {
            await _db.HashSetAsync(key, id, value);
        }

        public async Task<HashEntry[]?> GetHashSetAsync(string key)
        {
            return await _db.HashGetAllAsync(key);
        }

        public async Task<RedisValue> GetHashSetItemAsync(string key, int id)
        {
            return await _db.HashGetAsync(key, id);
        }
    }
}
