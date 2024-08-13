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
    string receivedString = Encoding.ASCII.GetString(receivedData);

    Console.WriteLine($"Received {receivedData.Length} bytes from {sourceEndPoint}: {receivedString}");

    byte[] response = Encoding.ASCII.GetBytes("");

    udpClient.Send(response, response.Length, sourceEndPoint);
}


