using System.Collections.Concurrent;

using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public class RosterStore : IRosterStore
    {
        private readonly ConcurrentDictionary<string, RosterEntryData> rosterData = new ConcurrentDictionary<string, RosterEntryData>();

        public IEnumerable<string> GetRosterNames()
        {
            return rosterData.Keys.ToList();
        }

        public void UpdateRoster(RosterEntryData data)
        {
            if(data.Name is null)
                throw new ArgumentException("Roster entry is missing name!",nameof(data));                

            rosterData[data.Name] = data;
        }
    }
}
