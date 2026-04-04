using System.Collections.Immutable;

namespace DiscordTrain.Common.Commands;

public interface IListStationsCommand
{
    Task<ImmutableList<IStation>> ListStationsAsync(CancellationToken cancellationToken);
}
