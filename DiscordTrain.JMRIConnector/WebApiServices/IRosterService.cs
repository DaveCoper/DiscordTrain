using DiscordTrain.Common.Model;
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.WebApiServices;

public interface IRosterService
{
    Task<IEnumerable<RosterEntryData>> GetRosterEntriesAsync(CancellationToken cancellationToken);
}