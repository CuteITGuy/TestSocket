using System;
using ServerSocket;


namespace ServerSocketConsole
{
    internal class Program
    {
        #region Event Handlers
        private static void Listener_Error(object sender, string e)
        {
            Console.WriteLine($"Error: {e}\r\n");
        }

        private static void Listener_MessageReceived(object sender, string e)
        {
            Console.WriteLine($"Message received: {e}");
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