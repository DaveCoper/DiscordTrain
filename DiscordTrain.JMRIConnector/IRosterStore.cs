using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public interface IRosterStore
    {
        void UpdateRoster(RosterEntryData data);

        IEnumerable<string> GetRosterNames();
    }
}