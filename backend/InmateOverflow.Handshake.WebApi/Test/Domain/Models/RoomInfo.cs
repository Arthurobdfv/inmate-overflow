namespace Test.Domain.Models
{
    public class RoomInfo
    {
        public uint RoomLeaderId { get; set; }
        public List<uint> RoomPlayers { get; set; } = new();
    }
}
