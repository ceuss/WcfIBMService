using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfIBMService
{
    interface IAplicacion
    {
        Task<string> GetRates();
        Task<string> GetTransactions();
        Task<SkuDetails> GetTransactionsListOf(string sku);
    }
}
