
using System.Net;
using System.Net.Sockets;
using System.Text;

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("127.0.0.1");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 6002);

using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);
while (true)
{
    // Send message.

   
    var message = $"PNG|UserName";
    var messageBytes = Encoding.UTF8.GetBytes(message + "\r\n");

    _ = await client.SendAsync(messageBytes, SocketFlags.None);
    Console.WriteLine($"Socket client sent message: \"{message}\" ");


    // Receive ack.
    var buffer = new byte[1024];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    var response = Encoding.UTF8.GetString(buffer, 0, received);
    var parameters = response.Split('|');
    if (parameters[0] == "#ACK")
    {
        Console.WriteLine($"Socket client received acknowledgment: \"{parameters[1]} - {parameters[2]}\" \n");
    }

    Thread.Sleep(2000);
}

