using CurrencyRateApp.Dto;
using CurrencyRateApp.Exceptions;
using CurrencyRateApp.Helpers;
using CurrencyRateApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ICacheService _cacheService;
        private readonly ICurrencyStatisticService _currencyStatisticService;
        private readonly ILogger<ExchangeRateService> _logger;

        private Dictionary<string, List<string>> _groupedCurrencyPairToDownload;
        private readonly List<CurrencyRatesDto> _downloadedCurrencyRates = new List<CurrencyRatesDto>();

        public ExchangeRateService(ILogger<ExchangeRateService> logger, ICacheService cacheService, ICurrencyStatisticService currencyStatisticService)
        {
            _cacheService = cacheService;
            _currencyStatisticService = currencyStatisticService;
            _logger = logger;
        }

        public async Task<List<CurrencyRatesDto>> GetCurrencyRatesAsync(CurrencyRateFilter currencyRateFilter)
        {
            _groupedCurrencyPairToDownload = currencyRateFilter.CurrencyCodes
                .GroupBy(cc => cc.Value)
                .ToDictionary(cc => cc.Key, g => g.Select(cc => cc.Key).ToList());
            _logger.LogInformation($"Grouped all currency codes by target currency");

            foreach (var targetCurrencyCode in _groupedCurrencyPairToDownload.Keys)
            {
                var currencyPairToDownload = new List<string>(_groupedCurrencyPairToDownload[targetCurrencyCode]);
                foreach (var sourceCurrencyCode in currencyPairToDownload)
                {
                    await GetExchangeRateFromCacheAsync(targetCurrencyCode, sourceCurrencyCode);
                }

                var sourceCurrencyCodesToFetchFromApi = _groupedCurrencyPairToDownload[targetCurrencyCode];
                if (sourceCurrencyCodesToFetchFromApi.Count > 0)
                {
                    await GetExchangeRateFromApiAsync(targetCurrencyCode, sourceCurrencyCodesToFetchFromApi);
                }
            }

            GetExchangeRateForSpecifiedPeriod(currencyRateFilter);

            return _downloadedCurrencyRates;
        }


        private async Task GetExchangeRateFromCacheAsync(string targetCurrencyCode, string sourceCurrencyCode)
        {
            var cacheKey = GetCacheKey(targetCurrencyCode, sourceCurrencyCode);

            var cachedCurrencyRates = await _cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedCurrencyRates))
            {
                _logger.LogInformation($"fetched data for {cacheKey} from cache");
                _groupedCurrencyPairToDownload[targetCurrencyCode].Remove(sourceCurrencyCode);
                CurrencyRatesDto result = JsonConvert.DeserializeObject<CurrencyRatesDto>(cachedCurrencyRates);
                _downloadedCurrencyRates.Add(result);
            }
        }

        private async Task GetExchangeRateFromApiAsync(string targetCurrencyCode, List<string> sourceCurrencyCodesToFetchFromApi)
        {
            string exchangeRates = await _currencyStatisticService.GetExchangeRateAsync(targetCurrencyCode, sourceCurrencyCodesToFetchFromApi);
            foreach (var singleCurrencyCode in sourceCurrencyCodesToFetchFromApi)
            {
                _logger.LogInformation($"Fetched data for {singleCurrencyCode}.{targetCurrencyCode} from API");
            }
            List<CurrencyRatesDto> result = CsvParsingHelper.ParseCsvResultToCurrencyRatesDtoList(exchangeRates);
            _logger.LogInformation($"Parsed fetched data to object");

            foreach (var sourceCurrencyCode in sourceCurrencyCodesToFetchFromApi)
            {
                string cacheKey = GetCacheKey(targetCurrencyCode, sourceCurrencyCode);
                var currencyRatesDto = result.FirstOrDefault(x => x.SourceCurrencyCode == sourceCurrencyCode);
                await _cacheService.CacheResponseAsync(cacheKey, currencyRatesDto, TimeHelper.GetTimeToMidnight());
                _logger.LogInformation($"Set data for {cacheKey} to cache");
                _downloadedCurrencyRates.Add(currencyRatesDto);
            }
        }

        private static string GetCacheKey(string targetCurrencyCode, string sourceCurrencyCode)
            => $"{sourceCurrencyCode}.{targetCurrencyCode}";

        private void GetExchangeRateForSpecifiedPeriod(CurrencyRateFilter currencyRateFilter)
        {
            if (_downloadedCurrencyRates.Count > 0)
            {
                var startDate = GetFirstDate(currencyRateFilter.StartDate.Date);
                _logger.LogInformation($"Set started date to: {startDate}");

                foreach (var currencyRate in _downloadedCurrencyRates)
                {
                    var currencyRatesFromSpecifiedPeriod = currencyRate.Values.Where(v => v.Date <= currencyRateFilter.EndDate.Date && v.Date >= startDate).ToList();
                    if (currencyRatesFromSpecifiedPeriod.Count == 0)
                    {
                        currencyRate.Message = "There is no exchangeRates for specified period";
                    }
                    currencyRate.Values = currencyRatesFromSpecifiedPeriod;
                }
                _logger.LogInformation($"Get exchange rate for specified period from: {startDate} to: {currencyRateFilter.EndDate.Date}");
            }
        }

        private DateTime GetFirstDate(DateTime startDate)
        {
            var firstCurrencyRateValues = _downloadedCurrencyRates.First().Values;
            var firstCurrencyRateDate = firstCurrencyRateValues.First().Date;
            if (firstCurrencyRateDate > startDate)
            {
                throw new BadRequestException("There is no historical data for this period");
            }
            else
            {
                var valuesForStartingDate = firstCurrencyRateValues.FirstOrDefault(v => v.Date == startDate);
                if (valuesForStartingDate == null)
                {
                    return GetFirstDate(startDate.AddDays(-1));
                }
                else
                {
                    return valuesForStartingDate.Date;
                }
            }
        }
    }
}
