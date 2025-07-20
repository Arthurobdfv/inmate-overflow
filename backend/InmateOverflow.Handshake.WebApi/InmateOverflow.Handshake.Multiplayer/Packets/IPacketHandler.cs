using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;

namespace InmateOverflow.Handshake.Multiplayer.Packets
{
    public interface IPacketHandler
    {
        Action<IParseablePacket> PacketHandler { get; set; }

        void Handle(byte[] packet, uint fromClient = 0);
    }
}
