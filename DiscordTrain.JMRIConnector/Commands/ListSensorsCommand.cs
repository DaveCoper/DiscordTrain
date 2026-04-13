using System.Collections.Immutable;

using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;
using DiscordTrain.JMRIConnector.WebApiServices;

namespace DiscordTrain.JMRIConnector.Commands;

public class ListSensorsCommand(ISensorService sensorService) : JmriCommand, IListSensorsCommand
{
    public async Task<ImmutableList<ISensorEntry>> ListSensorsAsync(CancellationToken cancellationToken)
    {
        var roster = await sensorService.GetSensorEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

        return roster.Select(x => new SensorEntry
        {
            Id = x.Name,
            Name = x.UserName,
            State = x.State
        }).ToImmutableList<ISensorEntry>();
    }
}
