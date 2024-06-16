using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DiscordTrain.JMRIConnector
{
    /*
public class JMRIWorkerService : BackgroundService, IThrottleProvider
{
    private readonly IJMRIConnection jmriConnection;
    private readonly IMessageRouter messageRouter;
    private readonly IRosterProvider rosterStore;
    private readonly ILoggerFactory loggerFactory;

    private readonly ConcurrentDictionary<string, JMRIThrottle> throttles;

    private readonly SemaphoreSlim throttleSemaphore = new SemaphoreSlim(1);

    public JMRIWorkerService(
        IJMRIConnection jmriConnection,
        IMessageRouter messageRouter,
        IRosterProvider rosterStore,
        ILoggerFactory loggerFactory)
    {
        this.jmriConnection = jmriConnection;
        this.messageRouter = messageRouter;
        this.rosterStore = rosterStore;

        this.loggerFactory = loggerFactory;
        this.throttles = new ConcurrentDictionary<string, JMRIThrottle>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.jmriConnection.ConnectAsync(stoppingToken);
        await ListenToMessages(stoppingToken);
    }

    public async ValueTask ListenToMessages(CancellationToken cancelationToken)
    {
        var buffer = new byte[10000];
        while (!cancelationToken.IsCancellationRequested)
        {
            var messages = await this.jmriConnection
                .ReceiveMessagesAsync(buffer, cancelationToken)
                .ConfigureAwait(false);

            foreach (var message in messages)
            {
                await messageRouter.RouteMessage(message)
                    .ConfigureAwait(false);
            }
        }
    }

    public async ValueTask<ITrainThrottle> GetThrottleAsync(string name)
    {
        if (throttles.TryGetValue(name, out var throttle))
        {
            return throttle;
        }

        try
        {
            if (throttles.TryGetValue(name, out throttle))
            {
                return throttle;
            }

            await throttleSemaphore.WaitAsync(TimeSpan.FromSeconds(1));

            string throttleId = Guid.NewGuid().ToString();
            throttle = new JMRIThrottle(
                throttleId,
                name,
                this.jmriConnection,
                loggerFactory.CreateLogger<JMRIThrottle>());

            throttles[name] = throttle;
            await throttle.RefreshValuesAsync();
            return throttle;
        }
        finally
        {
            throttleSemaphore.Release();
        }
    }
}
    */
}
