using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public class RosterService : IRosterProvider
    {
        private const string ServiceName = "roster";

        private readonly IJMRIApiClient apiClient;

        public RosterService(IJMRIApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<IEnumerable<RosterEntryData>> GetRosterEntriesAsync(CancellationToken cancellationToken)
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
