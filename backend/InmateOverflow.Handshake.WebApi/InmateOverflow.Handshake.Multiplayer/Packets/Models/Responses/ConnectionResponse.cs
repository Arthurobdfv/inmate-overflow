using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.IO;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses
{
    public class ConnectionResponse : IParseablePacket
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
            using var memStream = new MemoryStream();
            memStream.Write(Message.BytesFromUTF8());
            memStream.Write(PlayerId?.BytesFromUTF8());
            return memStream.ToArray();
        }
    }
}
