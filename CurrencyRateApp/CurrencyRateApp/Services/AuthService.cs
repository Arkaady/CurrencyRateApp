using CurrencyRateApp.Exceptions;
using CurrencyRateApp.Repositories;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHashService _hashService;
        private readonly IDatabaseRepository _databaseRepository;

        public AuthService(IHashService hashService, IDatabaseRepository databaseRepository)
        {
            _hashService = hashService;
            _databaseRepository = databaseRepository;
        }

        public async Task<string> GenerateApiKeyAsync()
        {
            try
            {
                Guid apiKey = Guid.NewGuid();
                var salt = _hashService.GetSalt();
                var apiKeyHash = _hashService.CalculateHash(salt, apiKey.ToString());

                var authorizationKey = await _databaseRepository.GetApiKeyAsync();
                if (authorizationKey == null)
                {
                    throw new BadRequestException("Authorization key not setted");
                }
                else
                {
                    authorizationKey.SetKey(salt, apiKeyHash);
                }

                await _databaseRepository.SetApiKeyAsync(authorizationKey);

                return apiKey.ToString();
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
                throw new BadRequestException("Error during generation new Api key");
            }
        }

        public async Task<bool> ValidateApiKey(string apiKey)
        {
            try
            {
                var authorizationKey = await _databaseRepository.GetApiKeyAsync();
                if (authorizationKey == null)
                {
                    throw new NotFoundException("Authorization key not setted");
                }
                var apiKeyHash = _hashService.CalculateHash(authorizationKey.Salt, apiKey);

                return apiKeyHash == authorizationKey.ApiKeyHash;
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
                throw new BadRequestException("Error during validation Api key");
            }
        }
    }
}
