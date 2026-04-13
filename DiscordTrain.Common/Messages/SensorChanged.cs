namespace DiscordTrain.Common.Messages;

public class SensorStateChanged
{
    public int SensorId { get; }

    public bool? SensorState { get; }

    public SensorStateChanged(int sensorId, bool? sensorState)
    {
        this.SensorId = sensorId;
        this.SensorState = sensorState;
    }
}