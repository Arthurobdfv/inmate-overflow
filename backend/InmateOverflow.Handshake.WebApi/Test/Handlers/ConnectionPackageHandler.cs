using InmateOverflow.Handshake.Multiplayer.Packets;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests;
using InmateOverflow.Handshake.Multiplayer.Packets.PacketHandlers;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using Test.Services;

namespace Test.Handlers
{
    public class ConnectionPackageHandler : BasePacketHandler<ConnectionRequest>
    {
        public HashSet<string> Players { get; set; }
        public Action<IParseablePacket> PacketHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ConnectionPackageHandler(MultiplayerContext context)
        {
        }

        public override void Handle(ConnectionRequest packet, uint fromClient = 0)
        {
            if (string.IsNullOrWhiteSpace(packet.PlayerId))
            {
                Guid guid = Guid.NewGuid();
            }
            Console.WriteLine($"Connection Packet Received with message: {packet.Message}");

        }
    }
}
