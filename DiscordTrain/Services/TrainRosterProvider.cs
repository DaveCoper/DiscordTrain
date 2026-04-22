using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;

namespace DiscordTrain.Services;

internal class TrainRosterProvider(IEnumerable<IListTrainsCommand> listRosterCommands) : ITrainRosterProvider
{
    public async Task<ImmutableList<ITrain>> GetTrainRosterAsync(CancellationToken cancellationToken)
    {
        var tasks = listRosterCommands.Select(x => x.GetTrainRosterAsync(cancellationToken));
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(x => x).ToImmutableList();
    }
}
