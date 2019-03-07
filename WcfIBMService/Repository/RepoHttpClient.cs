using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WcfIBMService
{
    public class RepoHttpClient : IRepository
    {
        protected static readonly Log4netIBM log = new Log4netIBM();

        #region Métodos de Rates
        public async Task<string> GetRates()
        {
            try
            {
                new FileInfo(Constants.ratesPath).Directory.Create();
                HttpClient Cliente = new HttpClient();
                var response = await Cliente.GetAsync(new Uri(Constants.urlRates));

                if (response.IsSuccessStatusCode)
                {
                    log.InfoMsg("Acceso correcto a la API");
                    var content = await response.Content.ReadAsStringAsync();
                    log.InfoMsg("Creando persistencia de ratios...");
                    File.WriteAllText(Constants.ratesPath, content);

                    return content;
                }
                else
                {
                    log.InfoMsg("No se pudo conectar a la API");
                    log.InfoMsg("Buscando persistencia de ratios...");

                    StreamReader file = new StreamReader(Constants.ratesPath);
                    string line = file.ReadLine();
                    file.Close();

                    return line;
                }
            }
            catch (UriFormatException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El formato de la uri proporcionada no es válida.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (ArgumentNullException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El argumento pasado al método es nulo.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (ArgumentException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El argumento pasado al método no es válido.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (FileNotFoundException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - No se encontró el fichero rates.json.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (IOException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - Error al intentar una operacion E/S sobre el fichero rates.json.");
                log.ErrorMsg(ex.ToString());
                return null;
            }

            catch (Exception ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - Se ha producido una excepción no esperada.");
                log.ErrorMsg(ex.ToString());
                return null;
            }

        }

        public async Task<List<Rate>> GetRatesList()
        {
            List<Rate> rates = new List<Rate>();
            rates = JsonConvert.DeserializeObject<List<Rate>>(await GetRates());
            if (rates.Count > 0)
                return rates;
            else
                return null;
        }

        public async Task<string> GetRatesFrom(string currency)
        {
            JArray arrayR = JArray.Parse(await GetRates());
            var rates = arrayR.Where(x => (string)x["from"] == currency);
            if (rates.Count() > 0)
                return rates.ToString();
            else
                return null;
        }
        #endregion

        #region Métodos de Transactions
        public async Task<string> GetTransactions()
        {
            try
            {
                new FileInfo(Constants.transPath).Directory.Create();
                HttpClient Cliente = new HttpClient();
                var response = await Cliente.GetAsync(new Uri(Constants.urlTrans));

                if (response.IsSuccessStatusCode)
                {
                    log.InfoMsg("Acceso correcto a la API");
                    log.InfoMsg("Creando persistencia de transacciones...");

                    var content = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(Constants.transPath, content);
                    return content;
                }
                else
                {
                    log.InfoMsg("No se pudo conectar a la API");
                    log.InfoMsg("Buscando persistencia de transacciones...");

                    StreamReader file = new StreamReader(Constants.transPath);
                    string line = file.ReadLine();
                    file.Close();

                    return line;
                }
            }
            catch (UriFormatException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El formato de la uri proporcionada no es válida.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (ArgumentException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El argumento pasado al método no es válido.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (FileNotFoundException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - No se encontró el fichero trans.json.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (IOException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - Error al intentar una operacion E/S sobre el fichero trans.json.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            catch (Exception ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - Se ha producido una excepción no esperada.");
                log.ErrorMsg(ex.ToString());
                return null;
            }

        }

        public async Task<SkuDetails> GetTransactionsOf(string sku)
        {
            SkuDetails detalles = await CalculateTransactions(MoneyConverter.GetMissingRates(await GetRatesList()), sku);
            if (detalles != null)
                return detalles;
            else
                return null;
        }

        private async Task<SkuDetails> CalculateTransactions(List<Rate> ratios, string sku)
        {
            try
            {
                List<Transaction> trans = JsonConvert.DeserializeObject<List<Transaction>>(await GetTransactions());
                SkuDetails detalles = new SkuDetails();
                if (trans.Count > 0)
                {
                    var querySkuTrans = from product in trans
                                        where product.sku == sku
                                        select product;
                    decimal suma = 0;
                    if (querySkuTrans.Count() > 0)
                    {
                        foreach (var item in querySkuTrans)
                        {
                            if (item.currency == Constants.currentCurrency)
                            {
                                detalles.transactions.Add(item);
                                decimal.TryParse(item.currency, out decimal sumaAux);
                                suma += sumaAux;
                            }
                            else
                            {
                                foreach (var r in ratios)
                                {
                                    if (r.from == item.currency && r.to == Constants.currentCurrency)
                                    {
                                        Transaction auxTrans = new Transaction();
                                        string auxR = r.rate.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                        string auxA = item.amount.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                        decimal.TryParse(auxR, out decimal auxRate);
                                        decimal.TryParse(auxA, out decimal auxAmount);
                                        auxTrans.sku = item.sku;
                                        auxTrans.currency = Constants.currentCurrency;

                                        auxTrans.amount = Math.Round(auxRate * auxAmount, Constants.toRound, MidpointRounding.ToEven).ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);
                                        
                                        suma += auxRate * auxAmount;
                                        detalles.transactions.Add(auxTrans);
                                    }
                                }
                            }
                        }
                        detalles.total = Math.Round(suma, Constants.toRound, MidpointRounding.ToEven).ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);
                        return detalles;
                    }
                    else
                    {
                        log.AlertMsg("No se ha encontrado el sku " + sku + ", se busca en el fichero de pruebas...");
                        try
                        {
                            // Se obtienen los rates del fichero de pruebas
                            StreamReader fileRates = new StreamReader(Constants.ratesPathPruebas);
                            string lineRates = fileRates.ReadLine();
                            fileRates.Close();
                            var ratesJson = JsonConvert.DeserializeObject<List<Rate>>(lineRates);
                            ratesJson = MoneyConverter.GetMissingRates(ratesJson);

                            // Se busca el sku en el fichero de pruebas
                            StreamReader fileTrans = new StreamReader(Constants.transPathPruebas);
                            string lineTrans = fileTrans.ReadLine();
                            fileTrans.Close();
                            var transJson = JsonConvert.DeserializeObject<List<Transaction>>(lineTrans);

                            if (transJson.Count > 0)
                            {
                                var querySkuTest = from item in transJson
                                                   where item.sku == sku
                                                   select item;

                                foreach (var item in querySkuTest)
                                {
                                    if (item.currency == Constants.currentCurrency)
                                    {
                                        detalles.transactions.Add(item);
                                        decimal.TryParse(item.currency, out decimal sumaAux);
                                        suma += sumaAux;
                                    }
                                    else
                                    {
                                        foreach (var r in ratesJson)
                                        {
                                            if (r.from == item.currency && r.to == Constants.currentCurrency)
                                            {
                                                Transaction auxTrans = new Transaction();
                                                string auxR = r.rate.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                                string auxA = item.amount.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                                decimal.TryParse(auxR, out decimal auxRate);
                                                decimal.TryParse(auxA, out decimal auxAmount);
                                                auxTrans.sku = item.sku;
                                                auxTrans.currency = Constants.currentCurrency;

                                                auxTrans.amount = Math.Round(auxRate * auxAmount, Constants.toRound, MidpointRounding.ToEven).ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);

                                                suma += auxRate * auxAmount;
                                                detalles.transactions.Add(auxTrans);
                                            }
                                        }
                                        detalles.total = Math.Round(suma, Constants.toRound, MidpointRounding.ToEven).ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);
                                        return detalles;
                                    }
                                }
                            }
                        }
                        catch (ArgumentNullException ex)
                        {
                            log.ErrorMsg("EXCEPTION ERROR");
                            log.ErrorMsg(ex.ToString());
                            return null;
                        }
                        catch (FileNotFoundException ex)
                        {
                            log.ErrorMsg("EXCEPTION ERROR");
                            log.ErrorMsg(ex.ToString());
                            return null;
                        }
                        catch (IOException ex)
                        {
                            log.ErrorMsg("EXCEPTION ERROR");
                            log.ErrorMsg(ex.ToString());
                            return null;
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                log.ErrorMsg("EXCEPTION ERROR - El argumento pasado al método es nulo.");
                log.ErrorMsg(ex.ToString());
                return null;
            }
            return null;
        }


        #endregion
    }
}