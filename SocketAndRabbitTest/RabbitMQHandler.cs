using RabbitMQ.Client;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Server
{
    public static class RabbitMQHandler
    {

        private static IModel _channel;
        static RabbitMQHandler()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "127.0.0.1" ,Port=5671};
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public static void Publish(StringPackageInfo package)
        {
            var queueName = $"queue.{package.Key}";

            _channel.QueueDeclare(queueName, true, false, false);
            _channel.QueueBind(queueName, "amq.topic", package.Key);

            var body = Encoding.UTF8.GetBytes(package.Body);

            _channel.BasicPublish("amq.topic", package.Key, null, body);
        }
    }
}
