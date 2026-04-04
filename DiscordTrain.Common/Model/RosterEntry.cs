using DiscordTrain.Common.Model;

namespace DiscordTrain.Common;

public class RosterEntry : IRosterEntry
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}
