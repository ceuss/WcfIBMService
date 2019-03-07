using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfIBMService
{
    public class Constants
    {
        #region Urls
        public const string urlRates = @"http://quiet-stone-2094.herokuapp.com/rates.json"; // URL de los rates de la app heroku formato json
        public const string urlTrans = @"http://quiet-stone-2094.herokuapp.com/transactions.json"; // URL de las transactions de la app heroku formato json
        #endregion

        #region Paths
        public const string ratesPath = @"./Data/rates.json"; // Ruta de persistencia de las conversiones
        public const string ratesPathPruebas = @"./Data/rates_pruebas.json"; // Ruta de conversiones para pruebas
        public const string transPath = @"./Data/transactions.json"; // Ruta de persistencia de las transacciones
        public const string transPathPruebas = @"./Data/transactions_pruebas.json"; // Ruta de persistencia de las transacciones
        #endregion

        #region Divisa y constantes de trabajo decimal
        public const string currentCurrency = "EUR"; // Divisa con la que se está trabajando actualmente
        public const string banker = "2";
        public const int toRound = 2;

        public const string sepDecimalEntrada = ".";
        public const string sepDecimalTrabajo = ",";
        #endregion
    }
}