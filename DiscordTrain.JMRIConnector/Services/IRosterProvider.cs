using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public interface IRosterProvider
    {
        Task<IEnumerable<RosterEntryData>> GetRosterEntriesAsync(CancellationToken cancellationToken);
    }
}