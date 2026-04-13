using System.Text.Json.Serialization;

using DiscordTrain.Common.Model;

namespace DiscordTrain.JMRIConnector.Messages;

public class SensorData
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("userName")]
    public required string UserName { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    //[JsonPropertyName("properties")]
    //public List<object> Properties { get; set; }

    [JsonPropertyName("inverted")]
    public bool Inverted { get; set; }

    [JsonPropertyName("state")]
    public SensorState State { get; set; }
}
