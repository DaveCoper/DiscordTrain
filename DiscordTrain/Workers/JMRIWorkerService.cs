using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.WebSocketServices;
using Microsoft.Extensions.Hosting;

namespace DiscordTrain.Workers
{
    public class JMRIWorkerService : BackgroundService
    {
        private readonly IJMRIWebSocketClient wsClient;

        private Timer timer;

        public JMRIWorkerService(IJMRIWebSocketClient wsClient)
        {
            this.wsClient = wsClient;
            this.wsClient.AddWeakMessageLisener(HandleHelloMessage);
        }

        private void HandleHelloMessage(object obj)
        {
            if (obj is JMRIMessage<HelloData> message)
            {
                timer?.Dispose();
                timer = new Timer(new TimerCallback(_ =>
                {
                    wsClient.SendAsync(new JMRIMessage { Type = "ping" }, CancellationToken.None);
                }), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(message.Data.Heartbeat));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await wsClient.InitializeAsync(stoppingToken);

            var buffer = new byte[10000];
            while (!stoppingToken.IsCancellationRequested)
            {
                await wsClient
                    .ProcessMessagesAsync(buffer, stoppingToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
