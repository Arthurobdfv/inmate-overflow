// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");



var hostName = Dns.GetHostName();
IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);

var listener = new TcpListener(IPAddress.Any, 13);
listener.Start();

var ConnectedHosts = new List<string>();
listener.BeginAcceptTcpClient(ConnectCallback, null);
void ConnectCallback(IAsyncResult ar)
{
    string hostName = ar.AsyncState as string;
    ConnectedHosts.Add(hostName);
}

Console.ReadLine();