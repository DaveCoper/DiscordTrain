namespace DiscordTrain.Common.Model;

public interface IRosterEntry
{
    string Id { get; }
    string Name { get; }
    string? SmallIcon { get; }
}
