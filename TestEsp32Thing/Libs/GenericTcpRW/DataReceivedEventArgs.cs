namespace TestEsp32Thing.Libs.GenericTcpRW
{
    public class DataReceivedEventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; } // readonly
    }
}