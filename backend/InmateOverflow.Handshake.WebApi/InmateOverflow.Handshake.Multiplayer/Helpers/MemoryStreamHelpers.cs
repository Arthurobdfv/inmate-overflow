using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using System.IO;
using Test.Services;

namespace InmateOverflow.Handshake.Multiplayer.Helpers
{
    public static class MemoryStreamHelpers
    {
        public static MemoryStream Push<T>(this MemoryStream memStream, T packet) where T : IParseablePacket {
            memStream.Write(packet.ToByteArray());
            return memStream;
        }

        public static MemoryStream Push(this MemoryStream memStream, string data)
        {
            var test = Convert.ToUInt32((int)data.Length);
            var stringLength = Convert.ToUInt32(data.Length);
            memStream.Push(stringLength);
            memStream.Write(data.BytesFromUTF8());
            return memStream;
        }

        public static string ReadString(this MemoryStream memStream)
        {
            var stringLength = memStream.ReadUInt32();
            
            var byteString = new byte[stringLength];
            memStream.Read(byteString);
            return byteString.UTF8FromBytes();
        }

        public static uint ReadUInt32(this MemoryStream memStream)
        {
            var size = sizeof(uint);
            var buffer = new byte[size];
            memStream.Read(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static MemoryStream Push(this MemoryStream memStream, uint data)
        {
            memStream.Write(BitConverter.GetBytes(data));
            return memStream;
        }

        public static MemoryStream Push(this MemoryStream memStream, int data)
        {
            memStream.Write(BitConverter.GetBytes(data));
            return memStream;
        }

        public static MemoryStream Push(this MemoryStream memStream, bool data)
        {
            memStream.Write(BitConverter.GetBytes(data));
            return memStream;
        }
    }
}
