using System.Collections.Generic;

namespace CurrencyRateApp.Dto
{
    public class CurrencyRatesDto
    {
        public string SourceCurrencyCode { get; set; }
        public string DestinationCurrencyCode { get; set; }
        public string Message { get; set; }
        public List<CurrencyValueDto> Values { get; set; } = new List<CurrencyValueDto>();
    }
}
