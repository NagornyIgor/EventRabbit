using System;

namespace EventConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageHandler = new QueueMessageHandler();
            messageHandler.StartProcessMessages();

            Console.WriteLine("Click enter to colse the program");
            Console.ReadLine();
        }
    }
}
