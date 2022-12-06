
using System.Net.Sockets;
using System.Net;
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

    var messageLGN = $"LGN|UserName|Password";
    var messageBytesLGN = Encoding.UTF8.GetBytes(messageLGN + "\r\n");

    _ = await client.SendAsync(messageBytesLGN, SocketFlags.None);
    Console.WriteLine($"Socket client sent message: \"{messageLGN}\" ");


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
