using System.Collections.Immutable;

using DiscordTrain.Common;
using DiscordTrain.Common.Commands;
using DiscordTrain.Common.Model;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.WebApiServices;

namespace DiscordTrain.JMRIConnector.Commands;

public class ListRosterCommand(IRosterService rosterService) : IListRosterCommand
{
    public async Task<ImmutableList<IRosterEntry>> ListRosterAsync(CancellationToken cancellationToken)
    {
        var roster = await rosterService.GetRosterEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

        return roster.Select(x => new RosterEntry
        {
            Id = GetFormatedId(x),
            Name = x.Name ?? string.Empty
        }).ToImmutableList<IRosterEntry>();
    }

    private string GetFormatedId(RosterEntryData x)
    {
        var prefix = x.IsLongAddress == true ? "L" : "S";
        return $"{prefix}{x.Address}";
    }
}
