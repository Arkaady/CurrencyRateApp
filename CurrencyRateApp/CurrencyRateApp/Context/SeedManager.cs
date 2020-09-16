using CurrencyRateApp.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRateApp.Context
{
    public class SeedManager
    {
        public EFContext DbContext { get; private set; }

        public SeedManager(EFContext dbContext)
        {
            DbContext = dbContext;
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
            var authorizationKey = new AuthorizationKey
            {
                ApiKeyHash = "Test_Api_Key"
            };

            await DbContext.AuthorizationKeys.AddAsync(authorizationKey);
            await DbContext.SaveChangesAsync();

            Log.Information("Database seeded successfuly");
        }
    }
}
