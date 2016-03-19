using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace SocketCommon
{
    public abstract class SocketBase
    {
        #region Fields
        private const int BACKLOG = 10;
        private const int BUFFER_SIZE = 1024;
        private const int PORT = 11000;
        private static readonly Encoding _defaultEncoding = Encoding.Default;
        private static readonly string _defaultEof = _defaultEncoding.GetString(new byte[] { 255, 0, 255, 0 });
        private static readonly IPHostEntry _hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        #endregion


        #region  Properties & Indexers
        public static string EndOfStream { get; set; } = _defaultEof;
        #endregion


        #region Events
        public event EventHandler<string> Error;
        public event EventHandler<string> MessageReceived;
        #endregion


        #region Methods
        public static IPEndPoint CreateEndPoint()
        {
            var ipAddress = _hostEntry.AddressList.Last();
            return new IPEndPoint(ipAddress, PORT);
        }

        public static Socket CreateListener()
        {
            var listener = CreateSocket();
            var endPoint = CreateEndPoint();
            listener.Bind(endPoint);
            listener.Listen(BACKLOG);
            return listener;
        }

        public static Socket CreateSender()
        {
            var sender = CreateSocket();
            var endPoint = CreateEndPoint();
            sender.Connect(endPoint);
            return sender;
        }

        public static Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion


        #region Implementation
        protected virtual string FetchMessage(Socket listener)
        {
            var sb = new StringBuilder();
            var buffer = new byte[BUFFER_SIZE];
            while (true)
            {
                var length = listener.Receive(buffer);
                var bufferString = _defaultEncoding.GetString(buffer, 0, length);
                sb.Append(bufferString);
                if (bufferString.EndsWith(EndOfStream)) break;
            }
            return sb.ToString(0, sb.Length - EndOfStream.Length);
        }

        protected virtual void PushMessage(Socket sender, string message)
        {
            sender.Send(Serialize(message));
        }

        protected virtual void OnError(string e)
        {
            Error?.Invoke(this, e);
        }

        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        protected virtual byte[] Serialize(string s)
        {
            return _defaultEncoding.GetBytes(s + EndOfStream);
        }
        #endregion
    }
}