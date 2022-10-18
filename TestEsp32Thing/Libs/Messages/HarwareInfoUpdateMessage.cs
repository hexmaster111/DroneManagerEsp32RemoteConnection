namespace TestEsp32Thing.Libs.Messages
{
    public class RegisterData
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public object Value { get; set; }
    }

    public class HardwareInfoUpdateMessage
    {
        public RegisterData[] Data { get; set; }
    }


    public enum DataType
    {
        Bit,
        Enum,
        Int,
        Float,
    }
}