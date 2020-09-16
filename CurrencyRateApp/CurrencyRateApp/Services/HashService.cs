using CurrencyRateApp.Services.Interfaces;
using System;
using System.Security.Cryptography;

namespace CurrencyRateApp.Services
{
    public class HashService : IHashService
    {
        private static readonly int DeriveBytesIterationCount = 1000;
        private static readonly int SaltSize = 16;

        public string GetSalt()
        {
            var saltBytes = new byte[SaltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }
        public string CalculateHash(string salt, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Can not generate hash from empty string");
            }
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentException("Can not use salt as empty string");
            }
            var pbkdf2 = new Rfc2898DeriveBytes(key, GetBytes(salt), DeriveBytesIterationCount);

            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}
