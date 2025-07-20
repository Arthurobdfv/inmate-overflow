namespace InmateOverflow.Handshake.Multiplayer.Models
{
    public class Packet
    {
        public byte[] Data { get; set; }
        public ushort Handler { get; set; }
    }
}
