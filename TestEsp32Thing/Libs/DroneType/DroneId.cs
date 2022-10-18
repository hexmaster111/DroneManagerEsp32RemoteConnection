using System;

namespace TestEsp32Thing.Libs.DroneType
{
    public class DroneId
    {
        private static readonly string _separator = "-";
        private static readonly string _prefix = "WTD";
        public DroneType Type { get; }
        public int Id { get; }


        public override bool Equals(object? obj)
        {
            var id = obj as DroneId;
            var same = id != null &&
                       Type == id.Type &&
                       Id == id.Id;

            return same;
        }


        public DroneId(DroneType type, int id)
        {
            Type = type;
            Id = id;
        }
    }
}