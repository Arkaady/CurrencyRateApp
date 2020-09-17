using System;

namespace CurrencyRateApp.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }


        public BadRequestException(string message, params object[] args)
            : base(message, args)
        {
        }

        public BadRequestException(Exception innerException, string message)
            : base(innerException, message)
        {
        }
    }
}
