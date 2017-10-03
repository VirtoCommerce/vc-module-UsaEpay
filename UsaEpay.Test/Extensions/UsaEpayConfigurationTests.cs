using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsaEpay.Extensions;

namespace UsaEpay.Tests.Extensions
{
    [TestClass]
    public class IntegerExtensionsTests
    {
        [TestMethod]
        public void ReturnTwoDigitMonthWhenLessThanTen()
        {
            int value = 1;
            var result = value.ToUsaEpayMonth();

            Assert.AreEqual("01", result);
        }

        [TestMethod]
        public void ReturnTwoDigitMonthWhenGreaterThanTen()
        {
            int value = 11;
            var result = value.ToUsaEpayMonth();

            Assert.AreEqual("11", result);
        }

        [TestMethod]
        public void ReturnTwoDigitYearWhenLessThanTen()
        {
            int value = 1;
            var result = value.ToUsaEpayYear();

            Assert.AreEqual("01", result);
        }

        [TestMethod]
        public void ReturnTwoDigitYearWhenGreaterThanTen()
        {
            int value =99;
            var result = value.ToUsaEpayYear();

            Assert.AreEqual("99", result);
        }
    }
}
