using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public class ThrottleService : IThrottleService
    {
        private const string ServiceName = "throttle";

        private readonly IJMRIApiClient apiClient;

        public ThrottleService(IJMRIApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public async ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, string rosterName, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = new ThrottleData
                {
                    RosterEntry = rosterName,
                    Throttle = throttleName,
                    Name = throttleName,
                }
            };

            return await PostThrottleDataAsync(message, cancellationToken);
        }

        public async ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, int trainAddress, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = new ThrottleData
                {
                    Address = trainAddress,
                    Throttle = throttleName,
                    Name = throttleName,
                }
            };

            return await PostThrottleDataAsync(message, cancellationToken);
        }

        public async ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = new ThrottleData
                {
                    Throttle = throttleName,
                    Name = throttleName,
                }
            };

            return await PostThrottleDataAsync(message, cancellationToken);
        }

        public async ValueTask<ThrottleData> SetThrottleDataAsync(ThrottleData throttleData, CancellationToken cancellationToken)
        {
            return await PostThrottleDataAsync(new JMRIMessage<ThrottleData>
            {
                Type = "throttle",
                Data = throttleData
            }, cancellationToken);
        }

        private async ValueTask<ThrottleData> PostThrottleDataAsync(
            JMRIMessage<ThrottleData> message,
            CancellationToken cancellationToken)
        {
            var result = await apiClient.PostAsync<JMRIMessage<ThrottleData>, JMRIMessage<ThrottleData>>(
                            ServiceName,
                            message,
                            cancellationToken);

            if (result == null)
                return message.Data;

            return result.Data;
        }
    }
}
