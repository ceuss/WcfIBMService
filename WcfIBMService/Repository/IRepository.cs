using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfIBMService
{
    interface IRepository
    {
        Task<string> GetRates(bool fichero = false);
        Task<List<Rate>> GetRatesList(bool fichero = false);
        Task<string> GetTransactions(bool fichero = false);
        Task<SkuDetails> GetTransactionsOf(string sku, bool fichero = false);
    }
}
