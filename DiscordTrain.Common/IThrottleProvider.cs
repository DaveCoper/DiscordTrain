namespace DiscordTrain.Common
{
    public interface IThrottleProvider
    {
        public ValueTask<ITrainThrottle> GetThrottleAsync(string name);
    }
}
