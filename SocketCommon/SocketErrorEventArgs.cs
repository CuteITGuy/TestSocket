using System;


namespace SocketCommon
{
    public class SocketErrorEventArgs: EventArgs
    {
        #region  Constructors & Destructor
        public SocketErrorEventArgs(string error)
        {
            Error = error;
        }
        #endregion


        #region  Properties & Indexers
        public string Error { get; set; }
        #endregion


        #region Override
        public override string ToString()
        {
            return Error;
        }
        #endregion
    }
}