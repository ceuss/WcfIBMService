namespace WcfIBMService
{
    public interface ILogIBM
    {
        void InfoMsg(string msg);
        void AlertMsg(string msg);
        void ErrorMsg(string msg);
    }
}
