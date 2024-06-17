using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public class ThrottleService : IThrottleService
    {
        private const string ServiceName = "throttle";

        private readonly IJMRIWebSocketClient wsClient;

        public ThrottleService(IJMRIWebSocketClient wsClient)
        {
            this.wsClient = wsClient;
        }

        public async ValueTask RegisterThrottleAsync(string throttleName, string rosterName, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = new ThrottleData
                {
                    RosterEntry = rosterName,
                    Name = throttleName,
                }
            };

            await SendThrottleDataAsync(message, cancellationToken);
        }

        public async ValueTask RegisterThrottleAsync(string throttleName, int trainAddress, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = new ThrottleData
                {
                    Address = trainAddress,
                    Name = throttleName,
                }
            };
            
            await SendThrottleDataAsync(message, cancellationToken);
        }

        public async ValueTask SetThrottleDataAsync(ThrottleData throttleData, CancellationToken cancellationToken)
        { 
            await SendThrottleDataAsync(new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = throttleData
            }, cancellationToken);
        }

        private async ValueTask SendThrottleDataAsync(
            JMRIMessage<ThrottleData> message,
            CancellationToken cancellationToken)
        {
            await wsClient.SendAsync(
                            message,
                            cancellationToken);
        }
    }
}