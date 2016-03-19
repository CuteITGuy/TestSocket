using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ServerSocket;
using SocketCommon;


namespace ServerSocketWindow
{
    public partial class MainWindow
    {
        #region Fields
        private readonly SocketListener _listener = new SocketListener();
        #endregion


        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
            _listener.MessageReceived += Listener_MessageReceived;
            _listener.Error += Listener_Error;
        }
        #endregion


        #region Event Handlers
        private void Listener_Error(object sender, SocketErrorEventArgs e)
        {
            txtStatus.Text = e.Error;
        }

        private void Listener_MessageReceived(object sender, SocketMessageEventArgs e)
        {
            txtMain.Text = e.ReceivedMessage;
            e.RespondedMessage = GetHash(e.ReceivedMessage);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => _listener.Listen());
        }
        #endregion


        #region Implementation
        private static string GetHash(string text)
        {
            using (var md5 = MD5.Create())
            {
                return string.Concat(md5.ComputeHash(Encoding.Default.GetBytes(text)).Select(b => b.ToString("x2")));
            }
        }
        #endregion
    }
}