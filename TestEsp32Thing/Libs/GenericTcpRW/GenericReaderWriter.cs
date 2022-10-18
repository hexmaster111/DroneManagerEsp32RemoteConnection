using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestEsp32Thing.Libs.GenericTcpRW
{
    public static class GenericReaderWriter
    {
        private static Thread ReaderThread;
        private static Thread WriterThread;

        public delegate void OnDataReceivedHandler(DataReceivedEventArgs e);

        public static event OnDataReceivedHandler OnDataReceived;

        public static void RaiseNewDataEvent(string data)
        {
            OnDataReceived?.Invoke(new DataReceivedEventArgs(data));
        }

        public static void SendMessage(object data, string target)
        {
            socketReaderThread.MessageQueue.AddMessage(data, target);
        }

        private static SocketReaderThread socketReaderThread;

        public static void Start(IPEndPoint serverIpEndPoint)
        {
            socketReaderThread = new SocketReaderThread(serverIpEndPoint);

            //TODO: Cleanup
            ReaderThread = new Thread(socketReaderThread.ReaderThreadFunction);
            WriterThread = new Thread(socketReaderThread.WriterThreadFunction);
            ReaderThread.Start();
            WriterThread.Start();
        }
    }
}