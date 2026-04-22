namespace DiscordTrain.Common.Model;

public interface ITrain
{
    string Id { get; }
    string Name { get; }
    string? SmallIcon { get; }
}
