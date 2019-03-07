using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WcfIBMService
{
    public class MoneyConverter
    {
        protected static readonly Log4netIBM log = new Log4netIBM();

        public static async Task<string> ConvertAmount(string fromCurrency, string toCurrency, string amount)
        {
            if (fromCurrency == toCurrency)
                return amount;

            Aplicacion api = new Aplicacion(new RepoHttpClient());

            var apiRates = await api.GetRatesList();
            var conversor = CurrencyConvert(apiRates, fromCurrency, toCurrency, amount).Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);

            double.TryParse(conversor, out double amountToRound);

            return Math.Round(amountToRound, Constants.toRound, MidpointRounding.ToEven).ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);
        }

        private static string CurrencyConvert(List<Rate> rates, string fromCurrency, string toCurrency, string amount)
        {
            if (fromCurrency == toCurrency)
                return amount;

            decimal suma = 0;
            foreach (var rate in rates)
            {
                if (rate.from == fromCurrency && rate.to == toCurrency)
                {
                    string auxAmount = amount.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                    decimal.TryParse(auxAmount, out decimal amountDecimal);
                    string auxRate = rate.rate.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                    decimal.TryParse(auxRate, out decimal rateDecimal);
                    suma = amountDecimal * rateDecimal;
                    return suma.ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);
                }
            }
            return null;
        }

        private static List<Rate> CalculateRates(List<Rate> rates)
        {
            int count = 0;
            List<Rate> ratiosAux = new List<Rate>(rates);

            do
            {
                count = ratiosAux.Count();
                ratiosAux = GetMissingRates(ratiosAux);
            } while (count != ratiosAux.Count());

            return ratiosAux;
        }

        public static List<Rate> GetMissingRates(List<Rate> rates)
        {
            try
            {
                List<string> divisas = rates.Select(x => x.from).Distinct().ToList();

                foreach (var d in divisas)
                {
                    List<Rate> divisasEntrada = rates.Where(x => x.from == d).ToList();
                    foreach (var dEntrada in divisasEntrada)
                    {
                        List<Rate> divisasSalida = rates.Where(x => x.from == dEntrada.to).ToList();
                        foreach (var dSalida in divisasSalida)
                        {
                            if (dSalida.to != dEntrada.from)
                            {
                                // crear nuevo rate
                                Rate rate = new Rate
                                {
                                    from = dEntrada.from,
                                    to = dSalida.to
                                };
                                string ratioEntrada = dEntrada.rate.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                string ratioSalida = dSalida.rate.Replace(Constants.sepDecimalEntrada, Constants.sepDecimalTrabajo);
                                decimal.TryParse(ratioEntrada, out decimal rdEntrada);
                                decimal.TryParse(ratioSalida, out decimal rdSalida);
                                decimal auxRate = rdEntrada * rdSalida;
                                rate.rate = auxRate.ToString().Replace(Constants.sepDecimalTrabajo, Constants.sepDecimalEntrada);

                                // añadirlo a la lista a retornar
                                if (rates.FirstOrDefault(r => r.from == rate.from && r.to == rate.to) == null)
                                    rates.Add(rate);
                            }
                        }
                    }
                }
                return rates;
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
        }
    }
}