using CurrencyRateApp.Models;
using System.Threading.Tasks;

namespace CurrencyRateApp.Repositories
{
    public interface IDatabaseRepository
    {
        Task SetApiKeyAsync(AuthorizationKey authKey);
        Task<AuthorizationKey> GetApiKeyAsync();
    }
}
