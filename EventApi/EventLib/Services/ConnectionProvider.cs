using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventLib.Services
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConfiguration _configuration;
        private IConnection Connection { get; }
        public IModel Chanel { get; }

        public ConnectionProvider(IConfiguration confuguration)
        {
            _configuration = confuguration;

            var connectionFactory = new ConnectionFactory() { HostName = _configuration["HostName"] };

            Connection = connectionFactory.CreateConnection();

            Chanel = Connection.CreateModel();

            Chanel.ExchangeDeclare(_configuration["ExchangeName"], ExchangeType.Direct);
        }
    }
}
