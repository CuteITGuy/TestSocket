using System;


namespace SocketCommon
{
    public class SocketMessageEventArgs: EventArgs
    {
        #region  Constructors & Destructor
        public SocketMessageEventArgs() { }

        public SocketMessageEventArgs(string receivedMessage)
        {
            ReceivedMessage = receivedMessage;
        }
        #endregion


        #region  Properties & Indexers
        public string ReceivedMessage { get; set; }
        public string RespondedMessage { get; set; }
        #endregion
    }
}