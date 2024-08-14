using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task<bool> CheckKeyExists(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task AddItemToHashSet(string key, int id, Product pr)
        {
            _db.HashSet(key, id, JsonSerializer.Serialize(product));
        }
    }
}
