using InmateOverflow.Handshake.Multiplayer.Domain.Constants.Enums;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses;
using InmateOverflow.Handshake.Multiplayer.Packets.PacketHandlers;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using Test.Domain.Models;
using Test.Helpers;
using Test.Services;

namespace Test.Handlers
{
    class CreateRoomHandler : BasePacketHandler<CreateRoomRequest>
    {
        private readonly LobbyManagementService _lobbyService;
        private MultiplayerContext _multiplayerContext;

        public Action<IParseablePacket> PacketHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CreateRoomHandler(LobbyManagementService lobbyService, MultiplayerContext context)
        {
            _lobbyService = lobbyService;
            _multiplayerContext = context;
        }

        public override void Handle(CreateRoomRequest packet, uint fromClient = 0)
        {
            if(fromClient == 0)
            {
                Console.WriteLine($"Error! Client with id 0 trying to create room!");
                return;
            }

            else
            {
                var clientExistingRoom = _lobbyService.GetClientRoom(fromClient);
                var guid = string.Empty;
                if (!string.IsNullOrWhiteSpace(clientExistingRoom))
                {
                    Console.Write($"Client {fromClient} already has room with id {clientExistingRoom} assigned");
                    guid = clientExistingRoom;
                }
                else
                {
                    guid = _lobbyService.CreateRoom(fromClient);
                    Console.Write($"Created room id \"{guid}\" for client {fromClient}");
                }
                _multiplayerContext.RespondSingle(fromClient, (ushort)PacketRouteEnum.CREATE_ROOM, new CreateRoomResponse() { RoomCode = guid });
            }
        }
    }
}
