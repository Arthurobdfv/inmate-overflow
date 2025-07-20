
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class GameClient : IDisposable
{
    TcpClient _socket;
    public TcpClient Socket => _socket;
    private readonly NetworkStream _stream;

    public GameClient(TcpClient client)
    {
        _socket = client;
        _stream = client.GetStream();
    }

    public async Task<string> Read(int bytes)
    {
        var buffer = new byte[bytes];
        int readBytes = await _stream.ReadAsync(buffer, 0, bytes);
        string request = Encoding.UTF8.GetString(buffer, 0, readBytes);
        return request;
    }

    public async Task<byte[]> ReadAsync()
    {
        byte[] data = new byte[1024];
        using MemoryStream memory = new MemoryStream(data);
        var read = await _stream.ReadAsync(data);
        return memory.ToArray();
    }

    public async Task Send(byte[] data)
    {
        await _stream.WriteAsync(data, 0, data.Length);
    }

    public async Task Send(string message)
    {
        byte[] response = Encoding.UTF8.GetBytes("Hello from server");
        await _stream.WriteAsync(response, 0, response.Length);
    }

    [Obsolete("Use method Send with destination")]
    public async Task Send(IParseablePacket packet)
    {
        byte[] response = packet.ToByteArray();
        await _stream.WriteAsync(response,0, response.Length);
    }

    public async Task Send(ushort destination, IParseablePacket packet)
    {
        using (var memStream = new MemoryStream())
        {
            Push(BitConverter.GetBytes(destination), memStream);
            var message = packet.ToByteArray();
            Push(message, memStream);
            var dataToWrite = memStream.ToArray();
            await _stream.WriteAsync(dataToWrite, 0, dataToWrite.Length);
        }
    }
    private void Push(byte[] data, Stream stream)
    {
        stream.Write(data, 0, data.Length);
    }

    public async Task<byte[]> ReceiveAsync()
    {
        using(var byteStream = new MemoryStream())
        {
            await _stream.CopyToAsync(byteStream);
            var data = byteStream.ToArray();
            return data;
        }
    }

    public uint GetId()
    {
        var ipAdress = ((IPEndPoint)Socket.Client.RemoteEndPoint!).Address;
        var idFromIp = BitConverter.ToUInt32(ipAdress.GetAddressBytes()) + (uint)((IPEndPoint)Socket.Client.RemoteEndPoint!).Port;
        return idFromIp;
    }

    public void Dispose()
    {
        _stream.Dispose();
        _socket.Dispose();
    }
}