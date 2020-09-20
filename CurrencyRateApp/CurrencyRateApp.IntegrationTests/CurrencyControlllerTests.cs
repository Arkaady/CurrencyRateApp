using CurrencyRateApp.Dto;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRateApp.IntegrationTests
{
    public class CurrencyControlllerTests : IntegrationTest
    {
        private readonly CurrencyRateFilter currencyRateFilter;
        public CurrencyControlllerTests()
        {
            currencyRateFilter = new CurrencyRateFilter
            {
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
                CurrencyCodes = new Dictionary<string, string>()
            };
            currencyRateFilter.CurrencyCodes.Add("USD", "EUR");
        }

        [Fact]
        public async Task GetCurrencyRateAsync_WithCorrectData_ShouldReturnCurrencyRates()
        {
            await SetApiKey();

            var stringQuery = $"StartDate={currencyRateFilter.StartDate:yyy-MM-dd}&EndDate={currencyRateFilter.EndDate:yyy-MM-dd}&CurrencyCodes[GBP]=EUR";
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
