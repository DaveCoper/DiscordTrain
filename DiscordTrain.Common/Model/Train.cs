using DiscordTrain.Common.Model;

namespace DiscordTrain.Common;

public class Train : ITrain
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? SmallIcon { get; set; }
}
