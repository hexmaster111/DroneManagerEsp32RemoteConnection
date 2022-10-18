using System.Net.Sockets;
using System.Text;
using nanoFramework.Json;

namespace TestEsp32Thing.Libs.GenericTcpRW
{
    public static class GenericWriter
    {
        public static void Write(object data, Socket socket, string targetContract)
        {
            var sendTarget = new SeasonableTarget
            {
                TargetInfo = targetContract,
                ContainedClass = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))
            };


            //Jsonify the sendTarget
            var sendJson = JsonConvert.SerializeObject(sendTarget);

            //Convert the json to a byte array
            var sendBytes = Encoding.UTF8.GetBytes(sendJson);
            socket.Send(sendBytes);
        }
    }
}