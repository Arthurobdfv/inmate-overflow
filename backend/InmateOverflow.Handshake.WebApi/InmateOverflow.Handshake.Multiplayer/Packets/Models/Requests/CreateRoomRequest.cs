using InmateOverflow.Handshake.Multiplayer.Services.Interface;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests
{
    public class CreateRoomRequest : IParseablePacket
    {
        public IParseablePacket FromByteArray(byte[] bytes)
        {
            return this;
        }

        public byte[] ToByteArray()
        {
            return new byte[0];
        }
    }
}
