﻿using System.Threading.Tasks;

namespace CurrencyRateApp.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<string> GenerateApiKeyAsync();
    }
}
