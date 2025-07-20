using InmateOverflow.Handshake.Multiplayer.Packets;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System.Net;

namespace Test.Services
{
    public class MultiplayerContext
    {
        private int Id { get; set; } = 0;
        private Dictionary<uint, GameClient> ConnectedHosts = new();
        public void RespondSingle(uint clientId, IncomingPacket data)
        {
            Task.Run(() => ConnectedHosts[clientId].Send(data.ToByteArray()));
        }

        public void RespondSingle(uint clientId, ushort direction, IParseablePacket packet)
        {
            RespondSingle(clientId, new IncomingPacket(direction, packet.ToByteArray()));
        }

        public void RespondLobby(IEnumerable<uint> clientsInLobby, IncomingPacket data)
        {
            foreach (var client in clientsInLobby) {
                RespondSingle(client, data);
            }
        }

        public void RespondLobby(IEnumerable<uint> clientsInLobby, ushort direction, IParseablePacket packet)
        {
            RespondLobby(clientsInLobby, new IncomingPacket(direction, packet.ToByteArray()));
        }

        public uint AddHost(GameClient gameClient)
        {
            var clientId = gameClient.GetId();
            if (ConnectedHosts.ContainsKey(clientId)) {
                throw new Exception("Client already connected to instance!");
            }
            ConnectedHosts.Add(clientId, gameClient);
            Console.WriteLine($"Host added Id: \"{clientId}\"");
            return clientId;
        }

        public void DisconnectClient(uint clientId) {
            if (ConnectedHosts.ContainsKey(clientId))
            {
                ConnectedHosts[clientId].Dispose();
                ConnectedHosts.Remove(clientId);
            }
        }

        public bool IsClientConnected(uint clientId) {
            return ConnectedHosts.ContainsKey(clientId);
        }

    }
}
