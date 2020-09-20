using System;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services.Interfaces
{
    public interface ICacheService
    {
        public Task CacheResponseAsync(string key, object value, TimeSpan? timeToLive = null);
        public Task<string> GetCachedResponseAsync(string key);
    }
}
