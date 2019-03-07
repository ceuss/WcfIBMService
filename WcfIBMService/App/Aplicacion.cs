using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfIBMService
{
    public class Aplicacion : IAplicacion
    {
        public Task<string> GetRates()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTransactions()
        {
            throw new NotImplementedException();
        }

        public Task<SkuDetails> GetTransactionsListOf(string sku)
        {
            throw new NotImplementedException();
        }
    }
}