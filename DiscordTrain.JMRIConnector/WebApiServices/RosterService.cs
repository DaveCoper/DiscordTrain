using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.WebApiServices
{
    public class RosterService : IRosterProvider
    {
        private const string ServiceName = "roster";

        private readonly IJMRIWebApiClient apiClient;

        public RosterService(IJMRIWebApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<IEnumerable<IRosterEntry>> GetRosterEntriesAsync(CancellationToken cancellationToken)
        {
            var messages = await apiClient.GetAsync<List<JMRIMessage<RosterEntryData>>>(ServiceName, cancellationToken);
            if (messages == null)
                return Enumerable.Empty<RosterEntryData>();

            var entries = messages
                .Select(x => x.Data)
                .ToList();

            return entries;
        }
    }
}
