using System.Collections.Immutable;

using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.WebApiServices
{
    public class SensorService : ISensorService
    {
        private const string ServiceName = "json/sensor";

        private readonly IJMRIWebApiClient apiClient;

        public SensorService(IJMRIWebApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ImmutableList<SensorData>> GetSensorEntriesAsync(CancellationToken cancellationToken)
        {
            var messages = await apiClient.GetAsync<List<JMRIMessage<SensorData>>>(ServiceName, cancellationToken);
            if (messages == null)
                return ImmutableList<SensorData>.Empty;

            var entries = messages
                .Select(x => x.Data)
                .ToList();

            return entries.ToImmutableList();
        }
    }
}
