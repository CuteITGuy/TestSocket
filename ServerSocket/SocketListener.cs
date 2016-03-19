using System;
using SocketCommon;


namespace ServerSocket
{
    public class SocketListener: SocketBase
    {
        #region Fields
        private bool _continued;
        #endregion


        #region Methods
        public void Listen()
        {
            try
            {
                _continued = true;
                ListenForMessage();
            }
            catch (Exception exception)
            {
                OnError(exception.Message);
            }
        }

        public void Stop()
        {
            _continued = false;
        }
        #endregion


        #region Implementation
        private void ListenForMessage()
        {
            using (var listener = CreateListener())
            {
                while (_continued)
                {
                    using (var handler = listener.Accept())
                    {
                        var message = FetchMessage(handler);
                        OnMessageReceived(message);
                        PushMessage(handler, "OK");
                    }
                }
            }
        }
        #endregion
    }
}