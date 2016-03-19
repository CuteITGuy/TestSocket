using System;
using System.Net.Sockets;
using System.Text;


namespace SocketCommon
{
    public class SocketFetcher: SocketHandlerBase
    {
        #region Fields
        private const int BUFFER_SIZE = 1024;
        private readonly byte[] _buffer = new byte[BUFFER_SIZE];
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Action<string> _fetchEndCallback;
        private readonly Socket _listener;
        #endregion


        #region  Constructors & Destructor
        public SocketFetcher(Socket listener, Action<string> fetchEndCallback)
        {
            _listener = listener;
            _fetchEndCallback = fetchEndCallback;
        }
        #endregion


        #region Methods
        public void BeginReceive()
        {
            _listener.BeginReceive(_buffer, 0, BUFFER_SIZE, SocketFlags.None, OnFetchEnd, null);
        }

        public void Receive()
        {
            while (true)
            {
                var length = _listener.Receive(_buffer);
                if (!Fetch(length)) continue;
                var message = GetMessage();
                _fetchEndCallback?.Invoke(message);
                return;
            }
        }
        #endregion


        #region Implementation
        private bool Fetch(int length)
        {
            var bufferString = _defaultEncoding.GetString(_buffer, 0, length);
            _builder.Append(bufferString);
            return bufferString.EndsWith(EndOfStream);
        }

        private string GetMessage()
        {
            return _builder.ToString(0, _builder.Length - EndOfStream.Length);
        }

        private void OnFetchEnd(IAsyncResult ar)
        {
            var length = _listener.EndReceive(ar);
            if (Fetch(length))
            {
                var message = GetMessage();
                _fetchEndCallback?.Invoke(message);
            }
            else
            {
                BeginReceive();
            }
        }
        #endregion
    }
}