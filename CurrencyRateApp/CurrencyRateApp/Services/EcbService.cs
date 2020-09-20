﻿using CurrencyRateApp.Exceptions;
using CurrencyRateApp.Services.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRateApp.Services
{
    public class EcbService : ICurrencyStatisticService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EcbService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExchangeRateAsync(string targetCurrencyCode, 
            List<string> sourceCurrencyCodes)
        {
            var httpClient = _httpClientFactory.CreateClient("ecb");
            var sourceCurrencyCodesSB = new StringBuilder();
            for (int i = 0; i < sourceCurrencyCodes.Count; i++)
            {
                sourceCurrencyCodesSB.Append(sourceCurrencyCodes[i]);
                if (i != sourceCurrencyCodes.Count - 1)
                {
                    sourceCurrencyCodesSB.Append("+");
                }
            }

            var finalUri = $"D.{sourceCurrencyCodesSB}.{targetCurrencyCode}.SP00.A";
            string result;
            try
            {
                Log.Information($"Send request to api with uri: {finalUri}");
                result = await httpClient.GetStringAsync(finalUri);
            }
            catch (HttpRequestException exception)
            {
                Log.Error(exception.Message);
                throw new BadRequestException($"error message from api: {exception.Message}, source currency: {sourceCurrencyCodesSB}, " +
                    $"targer currency: {targetCurrencyCode}");
            }

            return result;
        }
    }
}