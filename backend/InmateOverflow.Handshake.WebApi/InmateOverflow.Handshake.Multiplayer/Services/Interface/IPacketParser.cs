namespace InmateOverflow.Handshake.Multiplayer.Services.Interface
{
    public interface IPacketParser<T> where T : IParseablePacket
    {
        T ToPacket(byte[] data);
        byte[] ParsePacket(T packet);
    }
}
