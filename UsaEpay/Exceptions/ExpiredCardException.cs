using System;

namespace UsaEpay.Exceptions
{
    public class ExpiredCardException : Exception
    {
        public ExpiredCardException()
            : base("Credit card is expired.")
        {
        }
    }
}
