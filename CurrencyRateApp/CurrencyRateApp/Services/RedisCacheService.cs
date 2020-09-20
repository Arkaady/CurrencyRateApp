using CurrencyRateApp.Services.Interfaces;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class RedisCacheService : ICacheService, IDisposable
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task CacheResponseAsync(string key, object value, TimeSpan? timeToLive = null)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || value == null)
                {
                    return;
                }
                var serializedValue = JsonConvert.SerializeObject(value);
                var db = _connectionMultiplexer.GetDatabase();
                await db.StringSetAsync(key, serializedValue, timeToLive);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
            }
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public void Dispose()
        {
            _connectionMultiplexer.Dispose();
        }
    }
}
