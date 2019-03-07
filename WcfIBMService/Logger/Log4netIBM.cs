using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfIBMService
{
    public class Log4netIBM : ILogIBM
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void AlertMsg(string msg)
        {
            log.Warn(msg);
        }

        public void ErrorMsg(string msg)
        {
            log.Error(msg);
        }

        public void InfoMsg(string msg)
        {
            log.Info(msg);
        }
    }
}