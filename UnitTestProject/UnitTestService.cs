using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfIBMService;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestService
    {
        IServiceIBM service = new ServiceIBM();

        [TestMethod]
        public async Task TestMethodGetRatesAsync()
        {
            string g = await service.GetRates();
            Assert.AreEqual("[{\"from\":\"", g.Substring(0, 10));
        }

        [TestMethod]
        public async Task TestMethodGetTransAsync()
        {
            string g = await service.GetTransactions();
            Assert.AreEqual("[{\"sku\":\"", g.Substring(0, 9));
        }

        [TestMethod]
        public async Task TestMethodGetTransOf()
        {
            string g = await service.GetTransactionsOf("S9960");
            // Lo más probable es que no encuentre el sku proporcionado, por lo que si no lo encuentra lo buscará en fichero
            Assert.AreEqual("{\"transactions\":[{\"sku\":\"S9960\",\"amount\":\"39.56\",\"currency\":\"EUR\"}],\"total\":\"39.56\"}", g); //28.04\",\"currency\":\"EUR\"}],\"total\":\"28.04\"}
        }

    }
}
