// See https://aka.ms/new-console-template for more information
using InmateOverflow.Handshake.Multiplayer.Domain.Constants.Enums;
using InmateOverflow.Handshake.Multiplayer.Packets;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses;
using System.Net;
using System.Net.Sockets;
using Test.Handlers;
using Test.Services;

Console.WriteLine("Hello, World!");


var multiplayerContext = new MultiplayerContext();
var lobbyService = new LobbyManagementService(multiplayerContext);

Dictionary<ushort, IPacketHandler> handlers = new() 
{
    { (ushort)PacketRouteEnum.CONNECTION, new ConnectionPackageHandler(multiplayerContext) },
    { (ushort)PacketRouteEnum.CREATE_ROOM, new CreateRoomHandler(lobbyService, multiplayerContext) },
    { (ushort)PacketRouteEnum.CONNECT_TO_ROOM, new ConnectToRoomHandler(lobbyService, multiplayerContext) }
};

var hostName = Dns.GetHostName();
IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

using var listener = new TcpListener(IPAddress.Any, 13);
listener.Start();
var id = 0;

bool ServerUp = true;

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    await HandleClient(client);
}

async Task HandleClient(TcpClient client)
{
    var gameClient = new GameClient(client);
    id += 1;
    var clientId = multiplayerContext.AddHost(gameClient);
    StartListen(gameClient);
    multiplayerContext.RespondSingle(clientId, (ushort)PacketRouteEnum.CONNECTION, new ConnectionResponse { Message = "Hello from server!" });
}

void StartListen(GameClient gameClient)
{
    Task.Run(async () => await ListenLoop(gameClient, DataCallback, ErrorHandlerCallback));
}

async Task ListenLoop(GameClient gameClient, Action<byte[], uint> dataCallback, Action<Exception, uint> errorHandler)
{
    var clientId = gameClient.GetId();
    while (ServerUp && gameClient.Socket.Connected)
    {
        try
        {
            var incomingData = await gameClient.ReadAsync();
            dataCallback(incomingData, clientId);
        }
        catch(Exception e)
        {
            errorHandler(e, clientId);
        }
    }
}

void DataCallback(byte[] data, uint clientId)
{
    var packet = new IncomingPacket(data);
    Console.WriteLine($"Data Callback for {((PacketRouteEnum)(packet.PacketId)).ToString()} {(clientId != 0 ? $", from client \"{clientId}\"" : "")}");
    if (!handlers.ContainsKey((ushort)packet.PacketId))
    {
        Console.WriteLine($"Receiving packet with id {packet.PacketId} which is not in the dictionary!");
        multiplayerContext.DisconnectClient(clientId);
    }
    else
    {
        var handler = handlers[(ushort)packet.PacketId];
        handler.Handle(packet.Data, clientId);
    }
}

void ErrorHandlerCallback(Exception e, uint clientId)
{
    Console.WriteLine(e.Message);
    Console.WriteLine($"Error from client \"{clientId}\"");
    if(clientId != 0)
    {
        Console.WriteLine($"Disconnecting client {clientId}");
        multiplayerContext.DisconnectClient(clientId);
    }
}  

async Task ReceiveRecursive(GameClient gameClient)
{
    //var incomingData = await gameClient.Read();
    //var packet = new IncomingPacket(incomingData);
    //var handler = handlers[packet.PacketId];
    //handler.Handle(packet.Data);
}

Console.ReadLine();


