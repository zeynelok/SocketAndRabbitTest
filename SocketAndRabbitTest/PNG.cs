using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class PNG : IAsyncCommand<StringPackageInfo>
    {
        public ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        {
            var userName = package.Parameters[0];

            session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|PNG|{userName}"));

            RabbitMQHandler.Publish(package);

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"Recieved Message PNG: {userName}");
            Console.ResetColor();

            return ValueTask.CompletedTask;
        }
    
    }
}
