

using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Text;

namespace Server
{
    public class LGN : IAsyncCommand<StringPackageInfo>
    {
        public ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        {
            var userName = package.Parameters[0];
            var password = package.Parameters[1];
           
            session.SendAsync(Encoding.UTF8.GetBytes($"#ACK|LGN|{userName}"));

            RabbitMQHandler.Publish(package);

            Console.WriteLine($"Recieved Message LGN: {userName} / {password}");
         
            return ValueTask.CompletedTask;
        }
    }
}
