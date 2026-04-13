using System.Collections.Immutable;

using DiscordTrain.Common.Model;

namespace DiscordTrain.Common.Commands;

public interface IListRosterCommand : IConnectorCommand
{
    Task<ImmutableList<IRosterEntry>> ListRosterAsync(CancellationToken cancellationToken);
}
