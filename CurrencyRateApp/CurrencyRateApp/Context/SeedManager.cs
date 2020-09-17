using CurrencyRateApp.Models;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRateApp.Context
{
    public class SeedManager
    {
        public EFContext DbContext { get; private set; }
        public IHashService HashService { get; private set; }

        public SeedManager(EFContext dbContext, IHashService hashService)
        {
            DbContext = dbContext;
            HashService = hashService;
        }

        public async Task SeedDatabaseAsync()
        {
            await DbContext.Database.MigrateAsync();

            if (DbContext.AuthorizationKeys.Any())
            {
                return;
            }

            await SeedAsync();
        }

        private async Task SeedAsync()
        {
            var key = "95c9d5ad-bb45-480c-8568-44906592f75b";
            var salt = HashService.GetSalt();
            var apiKeyHash = HashService.CalculateHash(salt, key);
            var authenticationKey = new AuthorizationKey(salt, apiKeyHash);
            await DbContext.AuthorizationKeys.AddAsync(authenticationKey);
            await DbContext.SaveChangesAsync();

            Log.Information("Database seeded successfuly");
        }
    }
}
