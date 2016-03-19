using System;
using SocketCommon;


namespace SocketClient
{
    public class SocketSender: SocketBase
    {
        #region Methods
        public void Send(string message)
        {
            try
            {
                SendMessage(message);
            }
            catch (Exception exception)
            {
                OnError(exception.Message);
            }
        }
        #endregion


        #region Implementation
        private void SendMessage(string message)
        {
            using (var sender = CreateSender())
            {
                PushMessage(sender, message);
                message = FetchMessage(sender);
                OnMessageReceived(message);
            }
        }
        #endregion
    }
}