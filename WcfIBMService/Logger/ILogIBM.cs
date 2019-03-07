using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfIBMService
{
    public interface ILogIBM
    {
        void InfoMsg(string msg);
        void AlertMsg(string msg);
        void ErrorMsg(string msg);
    }
}
