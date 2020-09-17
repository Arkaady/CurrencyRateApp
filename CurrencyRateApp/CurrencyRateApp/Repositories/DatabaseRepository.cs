using CurrencyRateApp.Context;
using CurrencyRateApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CurrencyRateApp.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public EFContext DbContext { get; private set; }

        public DatabaseRepository(EFContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<AuthorizationKey> GetApiKeyAsync()
            => await DbContext.AuthorizationKeys.FirstOrDefaultAsync();

        public async Task SetApiKeyAsync(AuthorizationKey authKey)
        {
            DbContext.AuthorizationKeys.Update(authKey);
            await DbContext.SaveChangesAsync();
        }
    }
}
