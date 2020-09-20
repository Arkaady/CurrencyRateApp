using System;

namespace CurrencyRateApp.Helpers
{
    public static class TimeHelper
    {
        public static TimeSpan GetTimeToMidnight()
        {
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var midnight = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, tomorrow.Day, 0, 0, 0, 0);
            return midnight.Subtract(DateTime.UtcNow);
        }
    }
}
