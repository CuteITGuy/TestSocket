using System;
using SocketClient;
using SocketCommon;


namespace SocketClientConsole
{
    internal class Program
    {
        #region Event Handlers
        private static void Sender_Error(object sender, SocketErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.Error}");
        }

        private static void Sender_MessageReceived(object sender, SocketMessageEventArgs e)
        {
            Console.WriteLine($"Respond: {e.ReceivedMessage}");
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