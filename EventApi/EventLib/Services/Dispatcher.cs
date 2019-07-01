using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace EventLib.Services
{
    public class Dispatcher : IDispatcher
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionProvider _connectionProvider;

        public Dispatcher(IConnectionProvider connectionProvider, IConfiguration configuration)
        {
            _connectionProvider = connectionProvider;
            _configuration = configuration;
        }

        public void Send<T>(T message, IBasicProperties messageProperties)
        {      
             var routingKey = typeof(T).ToString();
             var messageContent = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            
            _connectionProvider.Chanel.BasicPublish(_configuration["ExchangeName"], routingKey, messageProperties, messageContent);
        }
    }
}
