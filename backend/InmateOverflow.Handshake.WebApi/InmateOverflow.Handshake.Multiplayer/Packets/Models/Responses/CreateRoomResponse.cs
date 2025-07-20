using InmateOverflow.Handshake.Multiplayer.Helpers;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.IO;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses
{
    public class CreateRoomResponse : IParseablePacket
    {
        public string RoomCode { get; set; }
        public IParseablePacket FromByteArray(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            RoomCode = stream.ReadString();
            return this;
        }

        public byte[] ToByteArray()
        {
            using var memStream = new MemoryStream();
            memStream.Push(RoomCode);
            return memStream.ToArray();
        }
    }
}
