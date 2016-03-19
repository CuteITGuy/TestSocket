using System;
using ServerSocket;
using SocketCommon;


namespace ServerSocketConsole
{
    internal class Program
    {
        #region Event Handlers
        private static void Listener_Error(object sender, SocketErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.Error}\r\n");
        }

        private static void Listener_MessageReceived(object sender, SocketMessageEventArgs e)
        {
            Console.WriteLine($"Message received: {e.ReceivedMessage}");
            e.RespondedMessage = "OK";
            AnnouceListening();
        }
        #endregion


        #region Implementation
        private static void AnnouceListening()
        {
            Console.WriteLine("Listening...");
        }

        private static void Main()
        {
            var listener = new SocketListener();
            listener.MessageReceived += Listener_MessageReceived;
            listener.Error += Listener_Error;
            AnnouceListening();
            listener.Listen();
            Console.Write("Enter to exit");
            Console.ReadLine();
        }
        #endregion
    }
}