using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfIBMService;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestAplication
    {
        Aplicacion app = new Aplicacion(new RepoHttpClient());

        [TestMethod]
        public async Task TestMethodGetRates()
        {
            var rates = await app.GetRates();
            Assert.AreEqual("[{\"from\":", rates.Substring(0, 9));
        }

        [TestMethod]
        public async Task TestMethodGetTransactions()
        {
            var trans = await app.GetTransactions();
            Assert.AreEqual("[{\"sku\":", trans.Substring(0, 8));
        }

        [TestMethod]
        public async Task TestMethodGetTransactionsOf()
        {
            var rates = await app.GetTransactionsOf("S9960");
            Assert.AreEqual("EUR", rates.transactions[0].currency);
            Assert.IsNotNull("EUR", rates.total);
        }
    }
}
