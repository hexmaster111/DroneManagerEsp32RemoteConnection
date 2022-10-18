using System;
using TestEsp32Thing.Libs.DroneType;

namespace TestEsp32Thing.Libs.Messages
{
    public class HandShakeMessage
    {
        public HandShakeMessage(DroneId id)
        {
            Id = id;
            TimeStamp = DateTime.UtcNow;
        }

        public HandShakeMessage(DroneId id, DateTime time)
        {
            Id = id;
            TimeStamp = time;
        }

        public DroneId Id { get; set; }
        public DateTime TimeStamp { get; set; }
    };
}