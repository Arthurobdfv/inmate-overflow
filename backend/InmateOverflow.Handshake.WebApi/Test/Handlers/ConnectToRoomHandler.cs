using InmateOverflow.Handshake.Multiplayer.Domain.Constants.Enums;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses;
using InmateOverflow.Handshake.Multiplayer.Packets.PacketHandlers;
using Test.Services;

namespace Test.Handlers
{
    public class ConnectToRoomHandler : BasePacketHandler<ConnectToRoomRequest>
    {
        private readonly LobbyManagementService _lobbyService;
        private readonly MultiplayerContext _multiplayerContext;
        public ConnectToRoomHandler(LobbyManagementService lobbyService, MultiplayerContext multiplayerContext)
        {
            _lobbyService = lobbyService;
            _multiplayerContext = multiplayerContext;
        }
        public override void Handle(ConnectToRoomRequest packet, uint fromClient = 0)
        {
            if (string.IsNullOrWhiteSpace(_lobbyService.GetRoom(packet.RoomToConnect)))
            {
                Console.WriteLine($"No room was found with roomId ${packet.RoomToConnect}");
            }
            if (fromClient == 0) {
                Console.WriteLine($"Client not yet connected!");
            }

            Console.WriteLine($"Connecting client {fromClient} to room {packet.RoomToConnect}");
            _lobbyService.LinkClientToRoom(fromClient, packet.RoomToConnect);
            var roomClients = _lobbyService.GetRoomClients(packet.RoomToConnect);
            var response = new ConnectToRoomResponse() { ClientsInRoom = roomClients.ToDictionary(x => x.ClientId, x => x.IsRoomLeader) };
            _multiplayerContext.RespondLobby(roomClients.Select(x => x.ClientId), (ushort)PacketRouteEnum.CONNECT_TO_ROOM ,response);
        }
    }
}
