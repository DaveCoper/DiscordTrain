using DiscordTrain.JMRIConnector.Messages;
using Microsoft.Extensions.Hosting;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    /*
    public class WorkerHeartbeatService : BackgroundService, IHeartbeatService
    {
        private int heartbeatInterval;

        private readonly IJMRIConnection jmriConnection;

        private SemaphoreSlim heartbeatSemaphore = new SemaphoreSlim(0);

        public WorkerHeartbeatService(IJMRIConnection jmriConnection)
        {
            heartbeatInterval = 0;

            this.jmriConnection = jmriConnection;
        }

        public async ValueTask SendPong()
        {
            await jmriConnection.SendAsync(new JMRIMessage { Type = "pong" });
        }
        public async ValueTask SendPing()
        {
            await jmriConnection.SendAsync(new JMRIMessage { Type = "ping" });
        }

        public void SetHeartbeatInterval(int heartbeatInterval)
        {
            this.heartbeatInterval = heartbeatInterval;
            if (heartbeatSemaphore.CurrentCount == 0)
                heartbeatSemaphore.Release();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await heartbeatSemaphore.WaitAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await SendPing();
                await Task.Delay(heartbeatInterval, stoppingToken);
            }
        }
    }
    */
}
