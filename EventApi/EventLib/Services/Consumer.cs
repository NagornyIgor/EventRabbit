using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace EventLib.Services
{
    public class Consumer : IConsumer
    {
        private readonly string ReplyQueueName;
        private readonly string CorelationId;
        private static readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly EventingBasicConsumer _consumer; 

        private readonly IConnectionProvider _connectionProvider;
        private readonly IConfiguration _configuration;

        public Consumer(IConnectionProvider connectionProvider, IConfiguration configuration)
        {
            _connectionProvider = connectionProvider;
            _configuration = configuration;

            ReplyQueueName = _connectionProvider.Chanel.QueueDeclare().QueueName;

            _connectionProvider.Chanel.QueueBind(ReplyQueueName, _configuration["ExchangeName"], ReplyQueueName);

            CorelationId = Guid.NewGuid().ToString();
            _consumer = new EventingBasicConsumer(_connectionProvider.Chanel);
            _consumer.Received += ProccessResponce;
        }

        public string Consume()
        { 
            _connectionProvider.Chanel.BasicConsume(ReplyQueueName, true, _consumer);
            return respQueue.Take();
        }

        private void ProccessResponce(object model, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId != CorelationId)
                return;

            var message = Encoding.UTF8.GetString(ea.Body);
            respQueue.Add(message);
        }

        public IBasicProperties GetResponceProperties()
        {
            var responceProperties = _connectionProvider.Chanel.CreateBasicProperties();
            responceProperties.ReplyTo = ReplyQueueName;
            responceProperties.CorrelationId = CorelationId;

            return responceProperties;
        }
    }
}
