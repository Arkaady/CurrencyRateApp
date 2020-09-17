using System;
using System.Globalization;

namespace CurrencyRateApp.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException()
        {
        }

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, params object[] args) 
            : base(string.Format(CultureInfo.InvariantCulture, message, args))
        {
        }

        protected BaseException(Exception innerException, string message) 
            : base(message, innerException)
        {
        }
    }
}
