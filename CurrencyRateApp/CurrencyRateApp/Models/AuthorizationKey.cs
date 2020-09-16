using System;

namespace CurrencyRateApp.Models
{
    public class AuthorizationKey
    {
        public int Id { get; set; }
        public string ApiKeyHash { get; private set; }
        public string Salt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private AuthorizationKey() { }

        public AuthorizationKey(string salt, string keyHash) : this()
        {
            SetKey(salt, keyHash);
        }

        public void SetKey(string salt, string keyHash)
        {
            SetApiKeyHash(keyHash);
            SetSalt(salt);
            CreatedAt = DateTime.UtcNow;
        }

        private void SetSalt(string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new Exception("Salt can not be empt");
            if (salt.Length > 255)
                throw new Exception("Salt can not be longer than 255 characters");
            if (salt == Salt)
                return;

            Salt = salt;
        }
        private void SetApiKeyHash(string keyHash)
        {
            if (string.IsNullOrEmpty(keyHash))
                throw new Exception("Key Hash can not be empt");
            if (keyHash.Length > 255)
                throw new Exception("Key Hash can not be longer than 255 characters");
            if (keyHash == ApiKeyHash)
                return;

            ApiKeyHash = keyHash;
        }
    }
}
