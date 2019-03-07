using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfIBMService
{
    public interface IAplicacion
    {
        Task<string> GetRates();
        Task<string> GetTransactions();
        Task<SkuDetails> GetTransactionsOf(string sku);
    }
}
