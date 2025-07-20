using InmateOverflow.Handshake.Multiplayer.Helpers;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses
{
    public class ConnectToRoomResponse : IParseablePacket
    {
        public Dictionary<uint, bool> ClientsInRoom { get; set; }
        public IParseablePacket FromByteArray(byte[] bytes)
        {
            using var memStream = new MemoryStream(bytes);
            ClientsInRoom = new Dictionary<uint, bool>();
            var arrayCount = new byte[sizeof(uint)];
            memStream.Read(arrayCount);
            var numOfElements = BitConverter.ToUInt32(arrayCount);
            var elementDataSize = sizeof(uint) + sizeof(bool);
            for(int i = 0; i < numOfElements; i++)
            {
                var data = new byte[elementDataSize];
                var elementData = memStream.Read(data);
                var clientId = BitConverter.ToUInt32(data, 0);
                var leader = BitConverter.ToBoolean(data, sizeof(uint));
                ClientsInRoom.Add(clientId, leader);
            }
            return this;
        }

        public byte[] ToByteArray()
        {
            using var memStream = new MemoryStream();
            memStream.Push(Convert.ToUInt32(ClientsInRoom.Count));
            foreach(var clients in ClientsInRoom)
            {
                memStream.Push(clients.Key);
                memStream.Push(clients.Value);
            }
            return memStream.ToArray();
        }
    }
}
