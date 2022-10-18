using System.Diagnostics;
using TestEsp32Thing.Libs.Eventer;
using TestEsp32Thing.Libs.Messages;
using System.Device.Gpio;
using TestEsp32Thing.Libs.GenericTcpRW;

namespace TestEsp32Thing.Libs.MessageHandling
{
    public class RequestHardwareUpdateMessageHandler : IObserver
    {
        public static RequestHardwareUpdateMessageHandler Instance { get; } = new RequestHardwareUpdateMessageHandler();

        private RequestHardwareUpdateMessageHandler()
        {
            // _pinA = _controller.OpenPin(10);
            // _pinB = _controller.OpenPin(11);

            // _pinA.SetPinMode(PinMode.Input);
            // _pinB.SetPinMode(PinMode.Input);
        }

        private static GpioController _controller = new GpioController();
        private static GpioPin _pinA;
        private static GpioPin _pinB;

        public void Update(byte[] value)
        {
            Debug.WriteLine("RequestHardwareUpdateMessageHandler: received message");
            //We dont need to do any with the value, we just need to send a message back to the server

            //Let just debug read a few pins and send that back
            var response = new HardwareInfoUpdateMessage
            {
                Data = new RegisterData[]
                {
                    new RegisterData
                    {
                        DataType = DataType.Bit,
                        Name = "Pin10",
                        Value = 1,
                    },
                    new RegisterData
                    {
                        DataType = DataType.Bit,
                        Name = "Pin11",
                        Value = 0
                    }
                }
            };
            
            //Todo send the value back to the server
            GenericReaderWriter.SendMessage(response, "HardwareInfoUpdate");
        }
    }
}