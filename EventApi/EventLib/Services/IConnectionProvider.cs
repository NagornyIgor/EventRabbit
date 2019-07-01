using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventLib.Services
{
    public interface IConnectionProvider
    {
        IModel Chanel { get; }
    }
}
