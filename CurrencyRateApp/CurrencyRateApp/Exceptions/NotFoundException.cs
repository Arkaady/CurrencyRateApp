namespace CurrencyRateApp.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, params object[] args)
            : base(message, args)
        {
        }
    }
}
