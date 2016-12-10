using System;
using System.Linq;
using Windows.Foundation.Collections;
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
            _connection.OnMessageReceived += ConnectionOnMessageReceived;
        }

        private async void ConnectionOnMessageReceived(ValueSet valueSet)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var message = valueSet.First();
                MessageResults.Text = $"{message.Value}";
            });
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
