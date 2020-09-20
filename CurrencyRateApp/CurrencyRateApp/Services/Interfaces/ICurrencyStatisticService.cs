﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services.Interfaces
{
    public interface ICurrencyStatisticService
    {
        public Task<string> GetExchangeRateAsync(string targetCurrencyCode, 
            List<string> sourceCurrencyCodes);
    }
}