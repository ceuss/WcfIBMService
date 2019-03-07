using System.Collections.Generic;
using System.Threading.Tasks;

namespace WcfIBMService
{
    interface IRepository
    {
        Task<string> GetRates();
        Task<List<Rate>> GetRatesList();
        Task<string> GetTransactions();
        Task<SkuDetails> GetTransactionsOf(string sku);
    }
}
