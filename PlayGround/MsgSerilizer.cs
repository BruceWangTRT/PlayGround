using System.IO;
using MsgPack.Serialization;

namespace PlayGround
{
    public class MsgSerilizer
    {
        public byte[] Serialize<T>(T value)
        {
            if (value == null) return default(byte[]);
            
            using (MemoryStream stream = new MemoryStream())
            {
                GetSerializer<T>().Pack(stream, value);
                var resp = stream.ToArray();
                return resp;
            }
        }
        public T Deserialize<T>(byte[] value, string bin = null)
        {

            if (value == null) return default(T);
            
            using (MemoryStream stream = new MemoryStream(value))
            {
                var result = GetSerializer<T>().Unpack(stream);
                return result;
            }
        }
        private MessagePackSerializer<T> GetSerializer<T>()
        {
            var serializer = SerializationContext.Default.GetSerializer<T>();
            return serializer;
        }
    }
}
