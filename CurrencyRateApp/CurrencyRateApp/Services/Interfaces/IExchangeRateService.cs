using CurrencyRateApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services.Interfaces
{
    public interface IExchangeRateService
    {
        public Task<List<CurrencyRatesDto>> GetCurrencyRatesAsync(CurrencyRateFilter currencyRateFilter);
    }
}
