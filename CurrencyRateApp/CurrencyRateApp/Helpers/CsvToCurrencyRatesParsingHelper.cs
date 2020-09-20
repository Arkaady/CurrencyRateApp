using CurrencyRateApp.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CurrencyRateApp.Helpers
{
    public static class CsvParsingHelper
    {
        public static List<CurrencyRatesDto> ParseCsvResultToCurrencyRatesDtoList(string csvResult)
        {
            string[] currencyRatesArray = csvResult.Split(Environment.NewLine);
            List<CurrencyRatesDto> exchangeRatesList = new List<CurrencyRatesDto>();
            for (int i = 1; i < currencyRatesArray.Length - 1; i++)
            {
                string[] singleLineDataArray = currencyRatesArray[i].Split(',');
                CurrencyRatesDto currency;
                if (i == 1)
                {
                    currency = MapNewCurrencyRatesDto(singleLineDataArray);
                    exchangeRatesList.Add(currency);
                }
                else
                {
                    currency = exchangeRatesList.Last();
                    if (singleLineDataArray[2] == currency.SourceCurrencyCode && singleLineDataArray[3] == currency.DestinationCurrencyCode)
                    {
                        CurrencyValueDto singleDayValue = MapSingleDayValue(singleLineDataArray);
                        currency.Values.Add(singleDayValue);
                    }
                    else
                    {
                        currency = MapNewCurrencyRatesDto(singleLineDataArray);
                        exchangeRatesList.Add(currency);
                    }
                }
            }

            return exchangeRatesList;
        }

        private static CurrencyValueDto MapSingleDayValue(string[] singleLineDataArray)
        {
            CurrencyValueDto singleDayValue = new CurrencyValueDto
            {
                Date = DateTime.ParseExact(singleLineDataArray[6], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Value = singleLineDataArray[7]
            };

            return singleDayValue;
        }

        private static CurrencyRatesDto MapNewCurrencyRatesDto(string[] singleLineDataArray)
        {
            CurrencyRatesDto currency = new CurrencyRatesDto
            {
                SourceCurrencyCode = singleLineDataArray[2],
                DestinationCurrencyCode = singleLineDataArray[3]
            };
            CurrencyValueDto singleDayValue = new CurrencyValueDto
            {
                Date = DateTime.ParseExact(singleLineDataArray[6], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Value = singleLineDataArray[7]
            };
            currency.Values.Add(singleDayValue);
            
            return currency;
        }
    }
}
