using InmateOverflow.Handshake.Multiplayer.Helpers;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.IO;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests
{
    public class ConnectToRoomRequest : IParseablePacket
    {
        public string RoomToConnect { get; set; }

        public IParseablePacket FromByteArray(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                RoomToConnect = stream.ReadString();
                return this;
            }
        }

        public byte[] ToByteArray()
        {
            using var memStream = new MemoryStream();
            memStream.Push(RoomToConnect);
            return memStream.ToArray();
        }
    }
}
