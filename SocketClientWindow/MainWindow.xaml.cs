using System.Threading.Tasks;
using System.Windows;
using SocketClient;
using SocketCommon;


namespace SocketClientWindow
{
    public partial class MainWindow
    {
        #region Fields
        private readonly SocketSender _sender = new SocketSender();
        #endregion


        #region  Constructors & Destructor
        public MainWindow()
        {
            InitializeComponent();
            _sender.MessageReceived += Sender_MessageReceived;
            _sender.Error += Sender_Error;
        }
        #endregion


        #region Event Handlers
        private void CmdSend_OnClick(object sender, RoutedEventArgs e)
        {
            var message = txtMain.Text;
            Task.Run(() => _sender.Send(message));
        }

        private void Sender_Error(object sender, SocketErrorEventArgs e)
        {
            txtError.Text = e.Error;
        }

        private void Sender_MessageReceived(object sender, SocketMessageEventArgs e)
        {
            txtSub.Text = e.ReceivedMessage;
        }
        #endregion
    }
}