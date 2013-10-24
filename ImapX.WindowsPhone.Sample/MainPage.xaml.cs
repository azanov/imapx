using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ImapX.Enums;
using Microsoft.Phone.Controls;

namespace ImapX.WindowsPhone.Sample
{
    public partial class MainPage : PhoneApplicationPage
    {

        private ImapClient _client;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
                _client = new ImapClient();
            else if(_client.IsConnected)
                _client.Disconnect();

            btnSignIn.IsEnabled = false;
            pgbMain.Visibility = Visibility.Visible;

            var count = Test();

            MessageBox.Show("Message count in folder \"All\": " + count);


            btnSignIn.IsEnabled = true;
            pgbMain.Visibility = Visibility.Collapsed;
        }

        private int Test()
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
                MessageBox.Show("Please enter a server");
            else if (string.IsNullOrWhiteSpace(txtPort.Text))
                MessageBox.Show("Please enter a port");
            else if (string.IsNullOrWhiteSpace(txtLogin.Text))
                MessageBox.Show("Please enter a login");
            else if (string.IsNullOrWhiteSpace(txtPassword.Password))
                MessageBox.Show("Please enter a password");
            else if (!_client.Connect(txtServer.Text, int.Parse(txtPort.Text), chkSSL.IsChecked.HasValue && chkSSL.IsChecked.Value, false))
                MessageBox.Show("Failed to establish connection");
            else if (!_client.Login(txtLogin.Text, txtPassword.Password))
                MessageBox.Show("Invalid credentials");
            else
            {
                // Do not download message data, only UIds to provide faster result
                _client.Behavior.MessageFetchMode = MessageFetchMode.None;
                var msgs = _client.Folders.All.Search();
                return msgs.Length;
            }
            return 0;
        }
    }
}