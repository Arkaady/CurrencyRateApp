using CurrencyRateApp.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRateApp.IntegrationTests
{
    public class CurrencyControlllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetCurrencyRateAsync_WithSingleCorrectCurrencyPair_ShouldReturnSingleCurrencyRates()
        {
            CurrencyRateFilter currencyRateFilter = CreateBasicCurrencyRateFilter();
            await SetApiKey();

            string stringQuery = CreateStringQuery(currencyRateFilter);
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");
            var respnseAsString = await response.Content.ReadAsStringAsync();
            var responseAsCurrencyRates = JsonConvert.DeserializeObject<List<CurrencyRatesDto>>(respnseAsString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseAsCurrencyRates.Count.Should().Be(1);
            responseAsCurrencyRates.First().SourceCurrencyCode.Should().Be("USD");
            responseAsCurrencyRates.First().DestinationCurrencyCode.Should().Be("EUR");
            responseAsCurrencyRates.First().Values.First().Date.Should().BeOnOrBefore(currencyRateFilter.StartDate);
            responseAsCurrencyRates.First().Values.Last().Date.Should().BeOnOrBefore(currencyRateFilter.EndDate);
        }

        [Fact]
        public async Task GetCurrencyRateAsync_WithTwoCorrectCurrencyPairs_ShouldReturnTwoCurrencyRates()
        {
            CurrencyRateFilter currencyRateFilter = CreateBasicCurrencyRateFilter();
            currencyRateFilter.CurrencyCodes.Add("PLN", "EUR");
            await SetApiKey();

            string stringQuery = CreateStringQuery(currencyRateFilter);
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");
            var respnseAsString = await response.Content.ReadAsStringAsync();
            var responseAsCurrencyRates = JsonConvert.DeserializeObject<List<CurrencyRatesDto>>(respnseAsString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseAsCurrencyRates.Count.Should().Be(2);
            responseAsCurrencyRates.First().SourceCurrencyCode.Should().Be("USD");
            responseAsCurrencyRates.First().DestinationCurrencyCode.Should().Be("EUR");
            responseAsCurrencyRates.Last().SourceCurrencyCode.Should().Be("PLN");
            responseAsCurrencyRates.Last().DestinationCurrencyCode.Should().Be("EUR");
            responseAsCurrencyRates.First().Values.First().Date.Should().BeOnOrBefore(currencyRateFilter.StartDate);
            responseAsCurrencyRates.First().Values.Last().Date.Should().BeOnOrBefore(currencyRateFilter.EndDate);
            responseAsCurrencyRates.First().Values.First().Date.Should().BeOnOrBefore(currencyRateFilter.StartDate);
            responseAsCurrencyRates.First().Values.Last().Date.Should().BeOnOrBefore(currencyRateFilter.EndDate);
        }

        [Fact]
        public async Task GetCurrencyRateAsync_WithDateFromTheFuture_ShouldReturnNotFound()
        {
            CurrencyRateFilter currencyRateFilter = CreateBasicCurrencyRateFilter();
            currencyRateFilter.EndDate = DateTime.UtcNow.AddDays(2);
            await SetApiKey();

            string stringQuery = CreateStringQuery(currencyRateFilter);
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCurrencyRateAsync_WithDateInFreeDayOfWorkFirstOfMay_ShouldReturnValuesFromLastDayOfApril()
        {
            CurrencyRateFilter currencyRateFilter = CreateBasicCurrencyRateFilter();
            currencyRateFilter.StartDate = new DateTime(2020, 5, 1);
            currencyRateFilter.EndDate = new DateTime(2020, 5, 1);
            await SetApiKey();

            string stringQuery = CreateStringQuery(currencyRateFilter);
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var respnseAsString = await response.Content.ReadAsStringAsync();
            var responseAsCurrencyRates = JsonConvert.DeserializeObject<List<CurrencyRatesDto>>(respnseAsString);

            responseAsCurrencyRates.Count.Should().Be(1);
            responseAsCurrencyRates.First().Values.First().Date.Should().Be(currencyRateFilter.StartDate.AddDays(-1));
        }

        [Fact]
        public async Task GetCurrencyRateAsync_WithVeryOldDate_ShouldReturnbadRequest()
        {
            CurrencyRateFilter currencyRateFilter = CreateBasicCurrencyRateFilter();
            currencyRateFilter.StartDate = new DateTime(1020, 5, 1);
            currencyRateFilter.EndDate = new DateTime(1021, 5, 1);
            await SetApiKey();

            string stringQuery = CreateStringQuery(currencyRateFilter);
            var response = await TestClient.GetAsync($"api/Currency?{stringQuery}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private CurrencyRateFilter CreateBasicCurrencyRateFilter()
        {
            var currencyRateFilter = new CurrencyRateFilter
            {
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow,
                CurrencyCodes = new Dictionary<string, string>()
            };
            currencyRateFilter.CurrencyCodes.Add("USD", "EUR");

            return currencyRateFilter;
        }

        private string CreateStringQuery(CurrencyRateFilter currencyRateFilter)
        {
            var stringQueryBuilder = new StringBuilder($"StartDate={currencyRateFilter.StartDate:yyy-MM-dd}&EndDate={currencyRateFilter.EndDate:yyy-MM-dd}");
            foreach (var key in currencyRateFilter.CurrencyCodes.Keys)
            {
                stringQueryBuilder.Append($"&CurrencyCodes[{key}]={currencyRateFilter.CurrencyCodes[key]}");
            }
            var stringQuery = stringQueryBuilder.ToString();
            return stringQuery;
        }
    }
}
