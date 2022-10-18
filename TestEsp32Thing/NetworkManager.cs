using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Networking;

namespace TestEsp32Thing
{
    public static class NetworkManager
    {
        public static void StartupScanConnect(WifiAdapter wifi, string mySsid, string myPassword)
        {
            try
            {
                // wifi.ScanAsync();

                if (!ConnectToNetwork(wifi, mySsid, myPassword))
                {
                    Debug.WriteLine("Could not connect to network, retrying in 5 seconds");
                    Thread.Sleep(5000);
                    ConnectToNetwork(wifi, mySsid, myPassword);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("message:" + ex.Message);
                Debug.WriteLine("stack:" + ex.StackTrace);
            }
        }

        //True on connected
        private static bool ConnectToNetwork(WifiAdapter network, string ssid, string password)
        {
            // Disconnect in case we are already connected
            network.Disconnect();

            // Connect to network
            // WifiConnectionResult result = network.Connect(ssid, WifiReconnectionKind.Automatic, password);

            var res = WifiNetworkHelper.ConnectDhcp(ssid, password, requiresDateTime: true);


            if (res == true)
            {
                Debug.WriteLine("Connected to network");
                return true;
            }
            else
            {
                Debug.WriteLine("Could not connect to network");
                return false;
            }

            // Display status
            // Debug.WriteLine(result.ConnectionStatus == WifiConnectionStatus.Success
            //     ? "Connected to Wifi network"
            //     : $"Error {result.ConnectionStatus.ToString()} connecting to Wifi network");
            // return result.ConnectionStatus == WifiConnectionStatus.Success;
        }
    }
}