using CurrencyRateApp.Context;
using CurrencyRateApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CurrencyRateApp.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly EFContext _dbContext;

        public DatabaseRepository(EFContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthorizationKey> GetApiKeyAsync()
            => await _dbContext.AuthorizationKeys.FirstOrDefaultAsync();

        public async Task SetApiKeyAsync(AuthorizationKey authKey)
        {
            _dbContext.AuthorizationKeys.Update(authKey);
            await _dbContext.SaveChangesAsync();
        }
    }
}
