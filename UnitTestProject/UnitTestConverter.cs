using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfIBMService;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestConverter
    {
        [TestMethod]
        public void TestCalculaRatesRestantes()
        {
            List<Rate> rates = new List<Rate>();
            Rate rate1 = new Rate
            {
                from = "CAD",
                to = "EUR",
                rate = "0.51"
            };
            rates.Add(rate1);
            Rate rate2 = new Rate
            {
                from = "EUR",
                to = "CAD",
                rate = "1.96"
            };
            rates.Add(rate2);
            Rate rate3 = new Rate
            {
                from = "CAD",
                to = "AUD",
                rate = "0.85"
            };
            rates.Add(rate3);
            Rate rate4 = new Rate
            {
                from = "AUD",
                to = "CAD",
                rate = "1.18"
            };
            rates.Add(rate4);
            Rate rate5 = new Rate
            {
                from = "EUR",
                to = "USD",
                rate = "1.1"
            };
            rates.Add(rate5);
            Rate rate6 = new Rate
            {
                from = "USD",
                to = "EUR",
                rate = "0.91"
            };
            rates.Add(rate6);

            rates = MoneyConverter.GetMissingRates(rates);

            Assert.AreEqual(12, rates.Count);
        }

        [TestMethod]
        public void TestCalculaRatesRestantesNulo()
        {
            Assert.IsNull(MoneyConverter.GetMissingRates(null));
        }

        [TestMethod]
        public async Task TestMethodConvertAmountAsync()
        {
            Assert.AreEqual("10", await MoneyConverter.ConvertAmount("CAD", "CAD", "10")); // Monto sin tratamiento

            Assert.IsNotNull(await MoneyConverter.ConvertAmount("CAD", "EUR", "10"));

        }

    }
}
