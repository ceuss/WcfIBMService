using System.ServiceModel;
using System.Threading.Tasks;

namespace WcfIBMService
{
    [ServiceContract]
    public interface IServiceIBM
    {
        [OperationContract]
        Task<string> GetRates();

        [OperationContract]
        Task<string> GetTransactions();

        [OperationContract]
        Task<string> GetTransactionsOf(string sku);
    }
}
