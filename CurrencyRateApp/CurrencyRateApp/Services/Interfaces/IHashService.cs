namespace CurrencyRateApp.Services.Interfaces
{
    public interface IHashService
    {
        public string GetSalt();
        public string CalculateHash(string salt, string key);
    }
}
