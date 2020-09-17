using CurrencyRateApp.Repositories;
using CurrencyRateApp.Services.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class AuthService : IAuthService
    {
        public IHashService HashService { get; private set; }
        public IDatabaseRepository DatabaseRepository { get; private set; }

        public AuthService(IHashService hashService, IDatabaseRepository databaseRepository)
        {
            HashService = hashService;
            DatabaseRepository = databaseRepository;
        }

        public async Task<string> GenerateApiKeyAsync()
        {
            try
            {
                Guid apiKey = Guid.NewGuid();
                var salt = HashService.GetSalt();
                var apiKeyHash = HashService.CalculateHash(salt, apiKey.ToString());

                var authorizationKey = await DatabaseRepository.GetApiKeyAsync();
                if (authorizationKey == null)
                {
                    throw new Exception("Authorization key not setted");
                }
                else
                {
                    authorizationKey.SetKey(salt, apiKeyHash);
                }

                await DatabaseRepository.SetApiKeyAsync(authorizationKey);

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
