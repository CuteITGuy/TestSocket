using SocketCommon;


namespace SocketClient
{
    public class SocketSender: SocketBase
    {
        #region Methods
        public void BeginSend(string message)
            => TryDo(() => BeginSendMessage(message));

        public void Send(string message) => TryDo(() => SendMessage(message));
        #endregion


        #region Implementation
        private void BeginSendMessage(string message)
        {
            var sender = CreateSender();
            BeginPushMessage(sender, message, () =>
            {
                BeginFetchMessage(sender, msg =>
                {
                    OnMessageReceived(new SocketMessageEventArgs(msg));
                    sender.Close();
                });
            });
        }

        private void SendMessage(string message)
        {
            var sender = CreateSender();
            PushMessage(sender, message, () =>
            {
                FetchMessage(sender, msg => OnMessageReceived(new SocketMessageEventArgs(msg)));
                sender.Close();
            });
        }
        #endregion
    }
}