using System.Collections.Immutable;

using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.WebApiServices;

public interface ISensorService
{
    Task<ImmutableList<SensorData>> GetSensorEntriesAsync(CancellationToken cancellationToken);
}