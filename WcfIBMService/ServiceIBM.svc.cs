using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WcfIBMService
{
    public class ServiceIBM : IServiceIBM
    {
        Aplicacion app = new Aplicacion(new RepoHttpClient());

        public Task<string> GetRates()
        {
            return app.GetRates();
        }

        public Task<string> GetTransactions()
        {
            return app.GetTransactions();
        }

        public async Task<string> GetTransactionsOf(string sku)
        {
            SkuDetails pd = await app.GetTransactionsOf(sku);
            return JsonConvert.SerializeObject(pd);
        }
    }
}
