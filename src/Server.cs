using codecrafters_dns_server.src;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


Console.WriteLine("Logs from your program will appear here!");


IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
int port = 2053;
IPEndPoint updEndPoint = new IPEndPoint(ipAddress, port);

UdpClient udpClient = new UdpClient(updEndPoint);

while (true)
{
    IPEndPoint sourceEndPoint = new IPEndPoint(IPAddress.Any, 0);
    byte[] receivedData = udpClient.Receive(ref sourceEndPoint);

    var Message = new DnsMessage(receivedData);

    byte[] response = Message.GetResponse();

    udpClient.Send(response, response.Length, sourceEndPoint);
}


