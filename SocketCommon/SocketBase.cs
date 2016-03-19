using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace SocketCommon
{
    public abstract class SocketBase
    {
        #region Fields
        private const int BACKLOG = 10;
        private const int PORT = 11000;

        private static readonly IPHostEntry _hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        protected readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        #endregion


        #region Events
        public event EventHandler<SocketErrorEventArgs> Error;
        public event EventHandler<SocketMessageEventArgs> MessageReceived;
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
        protected virtual void BeginFetchMessage(Socket listener, Action<string> fetchEndCallback = null)
        {
            new SocketFetcher(listener, fetchEndCallback).BeginReceive();
        }

        protected virtual void BeginPushMessage(Socket sender, string message, Action pushEndCallback = null)
            => new SocketPusher(sender, pushEndCallback).BeginSend(message);

        protected virtual void FetchMessage(Socket listener, Action<string> fetchEndCallback)
        {
            new SocketFetcher(listener, fetchEndCallback).Receive();
        }

        protected virtual void OnError(SocketErrorEventArgs e)
        {
            OnEventTriggered(Error, e);
        }

        protected virtual void OnEventTriggered<TEventArgs>(EventHandler<TEventArgs> evnt, TEventArgs e)
            where TEventArgs: EventArgs
        {
            if (_synchronizationContext != null)
                _synchronizationContext.Send(_ => evnt?.Invoke(this, e), null);
            else
                evnt?.Invoke(this, e);
        }

        protected virtual void OnMessageReceived(SocketMessageEventArgs e)
        {
            OnEventTriggered(MessageReceived, e);
        }

        protected virtual void PushMessage(Socket sender, string message, Action pushEndCallback = null)
            => new SocketPusher(sender, pushEndCallback).Send(message);

        protected virtual void TryDo(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                OnError(new SocketErrorEventArgs(exception.Message));
            }
        }
        #endregion
    }
}