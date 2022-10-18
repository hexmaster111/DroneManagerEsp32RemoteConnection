using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using nanoFramework.Json;
using nanoFramework.Networking;
using TestEsp32Thing.Libs.Eventer;
using TestEsp32Thing.Libs.GenericTcpRW;
using TestEsp32Thing.Libs.MessageHandling;

namespace TestEsp32Thing
{
    public class Program
    {
        private static string MySsid = "Homes Internet";
        private static string MyPassword = "letsgoontheinternet";

        private static IPAddress ServerIp = IPAddress.Parse("192.168.1.19");

        // private static IPAddress ServerIp = IPAddress.Parse("192.168.1.1");
        private static IPEndPoint ServerEndpoint = new IPEndPoint(ServerIp, 5000);
        private static NanoEventer Eventer = new NanoEventer();


        public static int Main()
        {
            // Get the first WiFI Adapter
            WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];
            NetworkManager.StartupScanConnect(wifi, MySsid, MyPassword);

            Eventer.Subscribe(RequestHardwareUpdateMessageHandler.Instance, "HardwareUpdateRequest");


            try
            {
                Debug.WriteLine("Connecting to server...");
                GenericReaderWriter.OnDataReceived += args =>
                {
                    // Debug.WriteLine("Received: " + args.Data);
                    var target = TargetDeserializer.Deserialize(args.Data);
                    Debug.WriteLine("Target: " + target.TargetInfo);
                    Eventer.OnEventFromSource(target);
                };

                GenericReaderWriter.Start(ServerEndpoint);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("**SocketException**: " + e.Message + "  Code: " + e.ErrorCode);
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"**STARTUP CRASH** Msg : {ex.Message}\r\nStack : {ex.StackTrace}\r\n Inner : {ex.InnerException}\r\n");
                return -2;
            }

            Debug.WriteLine("Everything Started... Main Thread exiting");
            return 0;
        }
    }
}