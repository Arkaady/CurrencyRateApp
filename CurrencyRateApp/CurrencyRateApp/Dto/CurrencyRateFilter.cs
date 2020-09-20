using System;
using System.Collections.Generic;

namespace CurrencyRateApp.Dto
{
    public class CurrencyRateFilter
    {
        public IDictionary<string, string> CurrencyCodes { get; set; } = new Dictionary<string, string>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CurrencyRateFilter()
        {
            StartDate = StartDate.Date;
            EndDate = EndDate.Date;
        }
    }
}
