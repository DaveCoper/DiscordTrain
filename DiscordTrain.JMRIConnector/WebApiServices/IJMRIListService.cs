using System.Collections.Immutable;

namespace DiscordTrain.JMRIConnector.WebApiServices;

public interface IJMRIListService<T>
{
    Task<ImmutableList<T>> GetEntriesAsync(CancellationToken cancellationToken);
}