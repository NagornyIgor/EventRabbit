using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EventConsole
{
    public class QueueMessageHandler
    {
        public static readonly ConnectionFactory _factory;
        public static readonly IConnection _connection;
        public readonly IModel _chanel;
        public readonly List<string> _messageTypes;

        static QueueMessageHandler()
        {
            _factory = new ConnectionFactory() { HostName = ConfigurationManager.AppSettings["HostName"] };
            _connection = _factory.CreateConnection();
        }

        public QueueMessageHandler()
        {
            _chanel = _connection.CreateModel();
            _chanel.ExchangeDeclare(ConfigurationManager.AppSettings["ExchangeName"], ExchangeType.Direct);
            _chanel.BasicQos(0, 1, false);

            _messageTypes = GetMessagesTypes();

            foreach (var messageType in _messageTypes)
            {
                _chanel.QueueDeclare(messageType);
                _chanel.QueueBind(messageType, ConfigurationManager.AppSettings["ExchangeName"], messageType);
            }
        }

        public void StartProcessMessages()
        {
            Console.WriteLine("Waiting for messages.");

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += DoSomeEventWork;

            foreach (var messageType in _messageTypes)
            {
                _chanel.BasicConsume(messageType, false, consumer);
            }
        }

        public void DoSomeEventWork(object model, BasicDeliverEventArgs ea)
        {
            string response = null;

            try
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                File.WriteAllText($"{ConfigurationManager.AppSettings["SavePath"]}\\{Guid.NewGuid().ToString()}.json", message);

                response = "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = "Error";
            }
            finally
            {
                var message = Encoding.UTF8.GetBytes(response);

                var replyProperties = _chanel.CreateBasicProperties();
                replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;

                _chanel.BasicPublish(ConfigurationManager.AppSettings["ExchangeName"], ea.BasicProperties.ReplyTo, false, replyProperties, message);
                _chanel.BasicAck(ea.DeliveryTag, false);
            }
        }

        private List<string> GetMessagesTypes()
        {
            return Assembly.Load("QueueLib").GetTypes().Where(t => t.IsClass && t.Namespace == "QueueLib")
                .Select(t => t.ToString())
                .ToList();
        }
    }
}
