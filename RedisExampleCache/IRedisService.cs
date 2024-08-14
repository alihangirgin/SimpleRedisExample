using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisExampleCache
{
    public interface IRedisService
    {
        Task<bool> CheckKeyExists(string key);
    }
}
