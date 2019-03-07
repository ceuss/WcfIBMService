using Newtonsoft.Json;
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
        Aplicacion app = new Aplicacion(new RepoHttpClient());

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

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
