using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Packets.PacketHandlers
{
    public abstract class BasePacketHandler<T> : IPacketHandler where T : IParseablePacket, new()
    {
        public Action<IParseablePacket> PacketHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Handle(byte[] packet, uint fromClient = 0)
        {
            Handle(packet.ToPacket<T>(), fromClient);
        }

        public abstract void Handle(T packet, uint fromClient = 0);
    }
}
