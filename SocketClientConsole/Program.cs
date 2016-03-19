using System;
using SocketClient;


namespace SocketClientConsole
{
    internal class Program
    {
        #region Event Handlers
        private static void Sender_Error(object sender, string e)
        {
            Console.WriteLine($"Error: {e}");
        }

        private static void Sender_MessageReceived(object sender, string e)
        {
            Console.WriteLine($"Respond: {e}");
        }
        #endregion


        #region Implementation
        private static void Main()
        {
            var sender = new SocketSender();
            sender.Error += Sender_Error;
            sender.MessageReceived += Sender_MessageReceived;
            while (true)
            {
                Console.Write("Message: ");
                var message = Console.ReadLine();
                sender.Send(message);
            }
        }
        #endregion
    }
}