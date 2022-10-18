using nanoFramework.Json;
using System;
using System.Diagnostics;

namespace TestEsp32Thing.Libs.GenericTcpRW
{
    public static class TargetDeserializer
    {
        public static object JsonLock = new object(); //This is a hack to get around the JsonLib not being thread safe

        public static SeasonableTarget Deserialize(string data)
        {

            SeasonableTarget target = null;

            lock (JsonLock)
            {
                try
                {
                    target = (SeasonableTarget)JsonConvert.DeserializeObject(data, typeof(SeasonableTarget));

                }
                catch (FormatException fex)
                {
                    Debug.WriteLine("Json was in the incorrect format\r\n" + fex.Message);
                    Debug.WriteLine(data);
                    target = null;
                }

            }

            if (target == null) { throw new System.Exception("Target was Null"); }

            return target;
        }
    }
}