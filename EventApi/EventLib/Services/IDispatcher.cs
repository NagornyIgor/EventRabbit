using RabbitMQ.Client;

namespace EventLib.Services
{
    public interface IDispatcher
    {
        void Send<T>(T message, IBasicProperties messageProperties);
    }
}
