using System.Collections.Immutable;

using DiscordTrain.Common;
using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.WebApiServices;

namespace DiscordTrain.JMRIConnector.Commands;

public class ListTrainsCommand(IRosterService rosterService) : JmriCommand, IListTrainsCommand
{
    public async Task<ImmutableList<ITrain>> GetTrainRosterAsync(CancellationToken cancellationToken)
    {
        var roster = await rosterService.GetRosterEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

        return roster.Select(x => new Train
        {
            Id = GetFormatedId(x),
            Name = x.Name ?? string.Empty,
            SmallIcon = x.Image
        }).ToImmutableList<ITrain>();
    }

    private string GetFormatedId(RosterEntryData x)
    {
        var prefix = x.IsLongAddress == true ? "L" : "S";
        return $"{prefix}{x.Address}";
    }
}
