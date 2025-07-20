using InmateOverflow.Handshake.Multiplayer.Packets;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Requests;
using InmateOverflow.Handshake.Multiplayer.Packets.Models.Responses;
using InmateOverflow.Handshake.Multiplayer.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class MultiplayerClient : MonoBehaviour
{
    public List<GameClient> GameClients = new List<GameClient>();
    Task waitingFirstMessage;
    Task listenTask;
    [SerializeField] static string RoomIdToConnect = "zI2vAd";
    string connectedRoom = string.Empty;

    Dictionary<uint, Action<byte[]>> Handlers = new Dictionary<uint, Action<byte[]>>()
    {
        { 1, HandleConnection },
        { 2, HandleCreateRoom },
        { 3, HandleConnectToRoom }
    };

    private static void HandleConnectToRoom(byte[] obj)
    {
        var connectToRoomResponse = ParseBytes<ConnectToRoomResponse>(obj);
        Debug.Log($"Connected successfully to room id ");
        Debug.Log($"Players found in the room: {string.Join(", ", connectToRoomResponse.ClientsInRoom.Select(x => $"Player Id: {x.Key}" + (x.Value ? " room leader" : "")))}");
    }

    string RoomCode = "zI2vAd";
    private static void HandleCreateRoom(byte[] packet)
    {
        var createRoomResponse = ParseBytes<CreateRoomResponse>(packet);
        Debug.Log($"Room Created! Code: {createRoomResponse.RoomCode}");
    }


    private static T ParseBytes<T>(byte[] data) where T : IParseablePacket, new()
    {
        return (T)new T().FromByteArray(data);
    }
    private static void HandleConnection(byte[] packet)
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    async Task Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConnectTo(false);
        }   
        if (Input.GetKeyDown(KeyCode.N))
        {
            ConnectToRoom();   
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateRoom();
        }
    }

    async Task StartServerListen(GameClient client)
    {
        var dataRead = new MemoryStream();
        listenTask = Task.Run(async () => await ReadLoop(ReadCallback, client, ErrorHandlerCallback));
        await client.Send((ushort)1, new ConnectionRequest() { Message = "Hello from client" });
    }

    async Task ReadLoop(Action<byte[]> dataRead, GameClient client, Action<Exception>? errorHandler = null)
    {
        var task = await client.ReadAsync();
        while (task.Length != 0)
        {
            try
            {
                task = await client.ReadAsync();
                dataRead(task);
            }
            catch (Exception e) {
                errorHandler?.Invoke(e);
                break;
            }
        }
    }

    void ReadCallback(byte[] data)
    {
        var packet = new IncomingPacket(data);
        if (Handlers.ContainsKey(packet.PacketId))
        {
            Handlers[packet.PacketId]?.Invoke(packet.Data);
        }
    }
    void ErrorHandlerCallback(Exception e)
    {
        Debug.LogError(e.ToString());
    }

    async Task ConnectTo(bool createNewCLient)
     {
        if (GameClients.Any() && !createNewCLient)
        {
            Debug.LogError("There is already a session attached to this instance");
            return;
        }
        var client = new TcpClient();
        //client.BeginConnect("localhost", 13, ConnectCallback, client);
        client.Connect("localhost", 13);
        GameClient gameClient = new(client);
        GameClients.Add(gameClient);
        //  StartCoroutine(StartServerListen());
        await StartServerListen(gameClient);
    }

    void CreateRoom()
    {
        var client = GameClients[0];
        Task.Run(async () => await client.Send(2, new CreateRoomRequest()));
    }

    void ConnectToRoom()
    {
        var client = GameClients[0];
        if (string.IsNullOrWhiteSpace(RoomCode))
        {
            Debug.LogError("Roomcode is empty");
        }
        else
        {
            Task.Run(() => client.Send(3, new ConnectToRoomRequest() { RoomToConnect = RoomIdToConnect }));
        }
    }

    private void Push(byte[] data, Stream stream)
    {
        stream.Write(data);
    }
}
