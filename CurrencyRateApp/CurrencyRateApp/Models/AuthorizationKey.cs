using System;

namespace CurrencyRateApp.Models
{
    public class AuthorizationKey
    {
        public int Id { get; set; }
        public string ApiKeyHash { get; set; }
        public DateTime CreatedAt { get; private set; }

        public AuthorizationKey()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
