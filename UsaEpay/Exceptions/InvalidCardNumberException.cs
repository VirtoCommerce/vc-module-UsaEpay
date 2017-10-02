using System;

namespace UsaEpay.Exceptions
{
    public class InvalidCardNumberException : Exception
    {
        public InvalidCardNumberException()
            : base("Credit card number is invalid.")
        {
        }
    }
}