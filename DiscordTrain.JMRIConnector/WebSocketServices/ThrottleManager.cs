using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using DiscordTrain.JMRIConnector.Services;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.Common;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public class ThrottleManager : IThrottleManager
    {
        private readonly IJMRIWebSocketClient wsClient;
        private readonly IThrottleService throttleService;
        private readonly ILoggerFactory loggerFactory;

        private readonly ConcurrentDictionary<string, IJMRIThrottle> throttlesByThrottleName;
        private readonly ConcurrentDictionary<string, IJMRIThrottle> throttlesByRosterName;

        private readonly SemaphoreSlim semaphore;

        public ThrottleManager(
            IJMRIWebSocketClient wsClient,
            IThrottleService throttleService,
            ILoggerFactory loggerFactory)
        {
            this.wsClient = wsClient;
            this.throttleService = throttleService;
            this.loggerFactory = loggerFactory;
            this.throttlesByRosterName = new ConcurrentDictionary<string, IJMRIThrottle>();
            this.throttlesByThrottleName = new ConcurrentDictionary<string, IJMRIThrottle>();
            this.semaphore = new SemaphoreSlim(1);

            this.wsClient.AddWeakMessageLisener(HandleThrottleMessages);
        }

        public ValueTask EmergencyStop()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ITrainThrottle> GetThrottleAsync(string rosterName, CancellationToken cancellationToken)
        {
            if (throttlesByRosterName.TryGetValue(rosterName, out var throttle))
            {
                return throttle;
            }

            await semaphore.WaitAsync();
            try
            {
                if (throttlesByRosterName.TryGetValue(rosterName, out throttle))
                {
                    return throttle;
                }
                var throttleName = $"Throttle{throttlesByRosterName.Count + 1}";
                throttle = new JMRIThrottle(
                    throttleName,
                    throttleService,
                    loggerFactory.CreateLogger<JMRIThrottle>());

                this.throttlesByRosterName[rosterName] = throttle;
                this.throttlesByThrottleName[throttleName] = throttle;
                await this.throttleService.RegisterThrottleAsync(throttleName, rosterName, cancellationToken);

                return throttle;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void HandleThrottleMessages(object message)
        {
            if (message is JMRIMessage<ThrottleData> throttleMessage)
            {
                var rosterName = throttleMessage.Data.Name;
                if (this.throttlesByThrottleName.TryGetValue(rosterName, out var throttle))
                {
                    throttle.UpdateThrottleData(throttleMessage.Data);
                }
            }
        }
    }
}