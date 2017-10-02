using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UsaEpay.Tests
{
    [TestClass]
    public class UsaEpayConfigurationTests
    {
        [TestMethod]
        public void ReturnSandboxAsDefault()
        {
            var configuration = new UsaEpayConfiguration();

            var dictionary = configuration.ToDictionary();

            Assert.AreEqual("sandbox", dictionary["mode"]);
        }

        [TestMethod]
        public void ReturnModeIfSet()
        {
            var configuration = new UsaEpayConfiguration { Mode = "SANDBOX" };

            var dictionary = configuration.ToDictionary();

            Assert.AreEqual("sandbox", dictionary["mode"]);
        }
    }
}
