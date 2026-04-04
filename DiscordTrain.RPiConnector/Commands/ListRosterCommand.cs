using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using DiscordTrain.Common;
using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;

using Microsoft.Extensions.Options;

namespace DiscordTrain.RPiConnector.Commands;

public class ListRosterCommand(IOptions<RPiConnectorOptions> options) : IListRosterCommand
{
    public Task<ImmutableList<IRosterEntry>> ListRosterAsync(CancellationToken cancellationToken)
    {
        var optionsValue = options.Value;
        var rosterEntry = new RosterEntry { Id = optionsValue.TrainId, Name = optionsValue.TrainName };
        return Task.FromResult(ImmutableList.Create<IRosterEntry>(rosterEntry));
    }
}
