using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DiscordTrain.Common;

namespace DiscordTrain.RPiConnector
{
    public class RpiRosterProvider : IRosterProvider
    {
        public Task<IEnumerable<IRosterEntry>> GetRosterEntriesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<IRosterEntry> entries = new[]
            {
                new RosterEntry{ 
                    Name = "Train", 
                    Model = "Gpio controlled train",
                    Address = 1,
                    Number = "1",
                    IsLongAddress = false,
                    Owner = "Me",
                    Road = "Home",
                },
            };

            return Task.FromResult(entries);
        }
    }
}
