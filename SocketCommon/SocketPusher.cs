using System;
using System.Net.Sockets;


namespace SocketCommon
{
    public class SocketPusher: SocketHandlerBase
    {
        #region Fields
        private readonly Action _pushEndCallback;
        private readonly Socket _sender;
        #endregion


        #region  Constructors & Destructor
        public SocketPusher(Socket sender, Action pushEndCallback = null)
        {
            _sender = sender;
            _pushEndCallback = pushEndCallback;
        }
        #endregion


        #region Methods
        public void BeginSend(string message)
        {
            var data = Serialize(message);
            _sender.BeginSend(data, 0, data.Length, SocketFlags.None, OnPushEnd, null);
        }

        public void Send(string message)
        {
            _sender.Send(Serialize(message));
            _pushEndCallback?.Invoke();
        }
        #endregion


        #region Implementation
        private void OnPushEnd(IAsyncResult ar)
        {
            _sender.EndSend(ar);
            _pushEndCallback?.Invoke();
        }

        protected virtual byte[] Serialize(string s)
        {
            return _defaultEncoding.GetBytes(s + EndOfStream);
        }
        #endregion
    }
}