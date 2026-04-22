using System.Collections.Immutable;

using DiscordTrain.Common.Model;

namespace DiscordTrain.Common.Commands;

public interface IListTrainsCommand : IConnectorCommand
{
    Task<ImmutableList<ITrain>> GetTrainRosterAsync(CancellationToken cancellationToken);
}
