using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.IO;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests
{
    public class ConnectionRequest : IParseablePacket
    {
        public string Message { get; set; }
        public string PlayerId { get; set; }
        public IParseablePacket FromByteArray(byte[] bytes)
        {
            this.Message = bytes.UTF8FromBytes();
            this.PlayerId = bytes.UTF8FromBytes();
            return this;
        }

        public byte[] ToByteArray()
        {
            using var stream = new MemoryStream();
            stream.Write(Message.BytesFromUTF8());
            stream.Write(PlayerId.BytesFromUTF8());
            return stream.ToArray();
        }
    }
}
