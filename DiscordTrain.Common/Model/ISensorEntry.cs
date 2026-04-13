namespace DiscordTrain.Common.Model;

public interface ISensorEntry
{
    string Id { get; }
    string Name { get; }
    SensorState State { get; }
}