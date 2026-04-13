using System.Collections.Immutable;

using DiscordTrain.Common.Model;

namespace DiscordTrain.Common.Commands;

public interface IListSensorsCommand : IConnectorCommand
{
    Task<ImmutableList<ISensorEntry>> ListSensorsAsync(CancellationToken cancellationToken);
}
