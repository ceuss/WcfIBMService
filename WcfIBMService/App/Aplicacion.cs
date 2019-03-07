using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfIBMService
{
    public class Aplicacion : IAplicacion
    {
        IRepository app;

        public Aplicacion(RepoHttpClient app)
        {
            this.app = app;
        }

        #region Rates
        public async Task<string> GetRates()
        {
            var contenido = await app.GetRates();
            return contenido;
        }

        public async Task<List<Rate>> GetRatesList()
        {
            List<Rate> rates = await app.GetRatesList();
            return rates;
        }
        #endregion

        #region Transactions
        public async Task<string> GetTransactions()
        {
            var contenido = await app.GetTransactions();
            return contenido;
        }

        public async Task<SkuDetails> GetTransactionsOf(string sku)
        {
            SkuDetails contenido = await app.GetTransactionsOf(sku);
            return contenido;
        }
        #endregion
    }
}