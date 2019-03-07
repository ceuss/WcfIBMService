using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WcfIBMService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServiceIBM : IServiceIBM
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Task<string> GetRates(bool fichero = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTransactions(bool fichero = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTransactionsOf(string sku, bool fichero = false)
        {
            throw new NotImplementedException();
        }
    }
}
