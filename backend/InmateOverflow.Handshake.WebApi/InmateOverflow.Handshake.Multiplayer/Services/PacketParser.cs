using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.Text;

namespace Test.Services
{
    public class PacketParser<T> : IPacketParser<T> where T : IParseablePacket, new()
    {
        public byte[] ParsePacket(T packet)
        {
            return packet.ToByteArray();
        }

        public T ToPacket(byte[] data)
        {
            var obj = new T().FromByteArray(data);
            return (T)obj;
        }
    }

    public static class Parser
    {
        public static T ToPacket<T>(this byte[] data) where T : IParseablePacket, new() => (T)(new T().FromByteArray(data));

        public static byte[] ParsePacket<T>(this T packet) where T : IParseablePacket, new() { return packet.ToByteArray(); } 

        public static string UTF8FromBytes(this byte[] data) => Encoding.UTF8.GetString(data);
        public static byte[] BytesFromUTF8(this string message) => Encoding.UTF8.GetBytes(message);
    }
}
