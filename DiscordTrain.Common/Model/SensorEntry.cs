namespace DiscordTrain.Common.Model;

public class SensorEntry : ISensorEntry
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public SensorState State { get; set; }
}
