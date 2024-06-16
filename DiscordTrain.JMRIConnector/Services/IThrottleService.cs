using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public interface IThrottleService
    {
        ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, CancellationToken cancellationToken);

        ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, int trainAddress, CancellationToken cancellationToken);

        ValueTask<ThrottleData> GetThrottleDataAsync(string throttleName, string rosterName, CancellationToken cancellationToken);

        ValueTask<ThrottleData> SetThrottleDataAsync(ThrottleData throttleData, CancellationToken cancellationToken);
    }
}