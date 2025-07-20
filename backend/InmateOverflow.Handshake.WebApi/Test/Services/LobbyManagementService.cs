using InmateOverflow.Handshake.Multiplayer.Domain.Constants;
using Test.Domain.Models;
using Test.Helpers;

namespace Test.Services
{
    public class LobbyManagementService
    {
        Dictionary<string, RoomInfo> Rooms = new();

        private readonly MultiplayerContext _context;
        public LobbyManagementService(MultiplayerContext context)
        {
            _context = context;
        }

        public string? GetRoom(string roomId) {
            return Rooms.ContainsKey(roomId) ? roomId : null;
        }

        internal void LinkClientToRoom(uint fromClient, string roomToConnect)
        {
            if (string.IsNullOrEmpty(roomToConnect)) {
                throw new ArgumentNullException(nameof(roomToConnect));
            }
            if (!Rooms.ContainsKey(roomToConnect))
            {
                throw new Exception($"Room with id \"{roomToConnect}\" not found.");
            }
            var room = Rooms[roomToConnect];

            if (!_context.IsClientConnected(room.RoomLeaderId))
            {
                throw new Exception($"Cannot reach to room leader...");
            }
            room.RoomPlayers.Add(fromClient);
        }

        internal IEnumerable<RoomClient> GetRoomClients(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                throw new ArgumentNullException(nameof(roomId));
            }
            if (!Rooms.ContainsKey(roomId))
            {
                throw new Exception($"Room with id \"{roomId}\" not found.");
            }
            var response = Rooms[roomId].RoomPlayers.Select(x => new RoomClient() { ClientId = x, IsRoomLeader = false })
                .Append(new RoomClient() { ClientId = Rooms[roomId].RoomLeaderId, IsRoomLeader = true });

            return response;
        }

        public string CreateRoom(uint clientLeader)
        {
            var guid = RoomKeyGenerator.GenerateCode(6);
            while (Rooms.ContainsKey(guid))
            {
                guid = RoomKeyGenerator.GenerateCode(6);
            }
            Rooms.Add(guid, new RoomInfo() { RoomLeaderId = clientLeader });
            return guid;
        }

        public string GetClientRoom(uint clientId)
        {
            var clientRoom = Rooms.Where(room => room.Value.RoomLeaderId == clientId).Select(e => (KeyValuePair<string, RoomInfo>?)e)
                .FirstOrDefault();
            return clientRoom?.Key;
        }
    }
}
