using RabbitMQ.Client;

namespace EventLib.Services
{
    public interface IConsumer
    {
        string Consume();

        IBasicProperties GetResponceProperties();
    }
}
