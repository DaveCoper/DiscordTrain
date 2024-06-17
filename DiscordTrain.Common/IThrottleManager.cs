namespace DiscordTrain.Common
{
    public interface IThrottleManager
    {
        ValueTask<ITrainThrottle> GetThrottleAsync(string rosterName, CancellationToken cancellationToken);

        ValueTask EmergencyStop();
    }
}