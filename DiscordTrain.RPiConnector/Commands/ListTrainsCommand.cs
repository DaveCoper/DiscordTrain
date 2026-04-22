using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using DiscordTrain.Common;
using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;

using Microsoft.Extensions.Options;

namespace DiscordTrain.RPiConnector.Commands;

public class ListTrainsCommand(IOptions<RPiConnectorOptions> options) : RPiCommand, IListTrainsCommand
{
    public Task<ImmutableList<ITrain>> GetTrainRosterAsync(CancellationToken cancellationToken)
    {
        var optionsValue = options.Value;
        var rosterEntry = new Train { Id = optionsValue.TrainId, Name = optionsValue.TrainName };
        return Task.FromResult(ImmutableList.Create<ITrain>(rosterEntry));
    }
}
