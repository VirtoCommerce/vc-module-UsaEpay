using System;

namespace UsaEpay.Extensions
{
    public static class DecimalExtensions
    {
        public static double ToDollars(this decimal value) => Convert.ToDouble(value);
    }
}