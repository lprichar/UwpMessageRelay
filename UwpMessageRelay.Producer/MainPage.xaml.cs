using System;
using Windows.UI.Xaml;
using UwpMessageRelay.Producer.Services;

namespace UwpMessageRelay.Producer
{
    public sealed partial class MainPage
    {
        private readonly MessageRelayService _connection = MessageRelayService.Instance;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageResults.Text = "Sending Message";
            try
            {
                await _connection.SendMessageAsync("Echo", Message.Text);
                MessageResults.Text = "Message Sent Successfully";
            }
            catch (Exception ex)
            {
                MessageResults.Text = "Send error: " + ex.Message;
            }
        }
    }
}
