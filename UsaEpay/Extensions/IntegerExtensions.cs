using System;

namespace UsaEpay.Extensions
{
    public static class IntegerExtensions
    {
        public static string ToUsaEpayMonth(this int value)
        {
            return AsTwoDigitValue(value);
        }

        public static string ToUsaEpayYear(this int value)
        {
            return AsTwoDigitValue(value);
        }

        private static string AsTwoDigitValue(int value)
        {
            return value.ToString("D2");
        }

    }
}