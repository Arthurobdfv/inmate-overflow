using System;
using System.IO;
using System.Linq;

namespace InmateOverflow.Handshake.Multiplayer.Packets
{
    public class IncomingPacket
    {
        public ushort PacketId { get; private set; }
        public uint FromClient {  get; private set; }
        public byte[] Data { get; private set; }

        public IncomingPacket(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var test = stream.ToArray();
                var dataLength = data.Length;
                var idLength = sizeof(ushort);
                var idBuffer = new byte[idLength];
                stream.Read(idBuffer, 0, idLength);
                PacketId = BitConverter.ToUInt16(idBuffer, 0);
                using(var remaningDataStream = new MemoryStream())
                {
                    stream.CopyTo(remaningDataStream);
                    Data = remaningDataStream.ToArray();
                }
            }
        }

        public IncomingPacket(ushort direction, byte[] data)
        {
            PacketId = direction;
            Data = data;
        }

        public IncomingPacket(uint fromClient, ushort direction, byte[] data)
        {
            FromClient = fromClient;
            PacketId = direction;
            Data = data;
        }

        public byte[] ToByteArray()
        {
            using var stream = new MemoryStream();
            stream.Write(BitConverter.GetBytes(PacketId));
            stream.Write(Data);
            return stream.ToArray();
        }
    }
}
