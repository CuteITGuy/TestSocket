using System;
using System.Net.Sockets;
using SocketCommon;


namespace ServerSocket
{
    public class SocketListener: SocketBase
    {
        #region Methods
        public void BeginListen() => TryDo(BeginListenForMessage);
        public void Listen() => TryDo(ListenForMessage);
        #endregion


        #region Implementation
        private void BeginListenForMessage()
        {
            using (var listener = CreateListener())
            {
                while (true)
                {
                    _resetEvent.Reset();
                    listener.BeginAccept(OnListenerAccepted, listener);
                }
            }
        }

        private void ListenForMessage()
        {
            using (var listener = CreateListener())
            {
                while (true)
                {
                    var handler = listener.Accept();
                    FetchMessage(handler, message =>
                    {
                        var args = new SocketMessageEventArgs(message);
                        OnMessageReceived(args);
                        PushMessage(handler, args.RespondedMessage);
                        handler.Close();
                    });
                }
            }
        }

        private void OnListenerAccepted(IAsyncResult ar)
        {
            _resetEvent.Set();
            var listener = ar.AsyncState as Socket;
            if (listener == null) return;

            var handler = listener.EndAccept(ar);

            BeginFetchMessage(handler, message =>
            {
                var args = new SocketMessageEventArgs(message);
                OnMessageReceived(args);
                BeginPushMessage(handler, args.RespondedMessage);
                handler.Close();
            });
        }
        #endregion
    }
}