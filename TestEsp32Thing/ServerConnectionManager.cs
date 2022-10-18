using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TestEsp32Thing.Libs;
using TestEsp32Thing.Libs.DroneType;
using TestEsp32Thing.Libs.GenericTcpRW;
using TestEsp32Thing.Libs.Messages;

namespace TestEsp32Thing
{
    public static class ServerConnectionManager
    {
        // public static void Test(Socket socket, IPEndPoint serverEndpoint)
        // {
        //     // GenericWriter.Write(new TestDto(123, "HelloWorld"), socket, "BadTarget");
        //     // GenericWriter.Write(new HandShakeMessage(new DroneId(DroneType.Experimental, 5050)), socket,
        //     //     "InitialConnectionHandShake");
        //
        //     GenericReader.OnDataReceived += GenericReaderOnOnDataReceived;
        //
        //     GenericReader.Start(serverEndpoint);
        // }

        private static void GenericReaderOnOnDataReceived(DataReceivedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}