using System;
using System.Linq;
using System.Threading.Tasks;
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
            Loaded += OnLoaded;
            _connection.OnMessageReceived += ConnectionOnMessageReceived;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await EnsureConnected();
        }

        private async Task EnsureConnected()
        {
            var retryDelay = 10000;
            await Task.Delay(retryDelay);
            while (!_connection.IsConnected)
            {
                try
                {
                    await _connection.Open();
                }
                catch (Exception)
                {
                    // note ensure MessageRelay is deployed by right clicking on UwpMessageRelay.MessageRelay and selecting "Deploy"
                    MessageResults.Text = $"Unable to connect to siren of shame engine. Retrying in {(retryDelay / 1000)} seconds...";
                    await Task.Delay(retryDelay);
                }
            }
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
