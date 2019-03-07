using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfIBMService
{
    public class SkuDetails
    {
        public List<Transaction> transactions = new List<Transaction>();
        public string total;
    }
}