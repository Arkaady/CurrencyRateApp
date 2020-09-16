using CurrencyRateApp.Context;
using CurrencyRateApp.Models;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class AuthService : IAuthService
    {
        public IHashService HashService { get; private set; }
        public EFContext DbContext { get; private set; }

        public AuthService(IHashService hashService, EFContext dbContext)
        {
            HashService = hashService;
            DbContext = dbContext;
        }

        public async Task<string> GenerateApiKeyAsync()
        {
            try
            {
                Guid apiKey = Guid.NewGuid();
                var salt = HashService.GetSalt();
                var apiKeyHash = HashService.CalculateHash(salt, apiKey.ToString());

                var authorizationKey = await DbContext.AuthorizationKeys.FirstOrDefaultAsync();
                if (authorizationKey == null)
                {
                    throw new Exception("Authorization key not setted");
                }
                else
                {
                    authorizationKey.SetKey(salt, apiKeyHash);
                }

                DbContext.AuthorizationKeys.Update(authorizationKey);
                await DbContext.SaveChangesAsync();

                return apiKey.ToString();
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
                throw new Exception("Error during generation new Api key");
            }

        }
    }
}
