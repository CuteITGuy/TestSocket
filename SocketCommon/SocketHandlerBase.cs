using System.Text;


namespace SocketCommon
{
    public abstract class SocketHandlerBase
    {
        #region Fields
        protected static readonly Encoding _defaultEncoding = Encoding.Default;
        private static readonly string _defaultEof = _defaultEncoding.GetString(new byte[] { 255, 0, 255, 0 });
        #endregion


        #region  Properties & Indexers
        protected static string EndOfStream { get; } = _defaultEof;
        #endregion
    }
}