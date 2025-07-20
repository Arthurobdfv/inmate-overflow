namespace InmateOverflow.Handshake.Multiplayer.Services.Interface
{
    public interface IParseablePacket
    {
        IParseablePacket FromByteArray(byte[] bytes);
        byte[] ToByteArray();
    }
}
