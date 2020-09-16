using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRateApp.Dto
{
    public class CurrencyRateRequestDto
    {
        public Dictionary<string, string> CurrencyCodes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
