using System.Collections.Immutable;

using DiscordTrain.Common.Model;

namespace DiscordTrain.Common.Commands;

public interface IListRosterCommand
{
    Task<ImmutableList<IRosterEntry>> ListRosterAsync(CancellationToken cancellationToken);
}
