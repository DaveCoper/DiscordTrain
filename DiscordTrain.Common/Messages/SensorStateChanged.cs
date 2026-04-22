using DiscordTrain.Common.Model;

namespace DiscordTrain.Common.Messages;

public class SensorStateChanged
{
    public string SensorId { get; }

    public SensorState SensorState { get; }

    public SensorStateChanged(string sensorId, SensorState sensorState)
    {
        this.SensorId = sensorId;
        this.SensorState = sensorState;
    }
}