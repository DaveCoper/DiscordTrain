using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public interface IThrottleService
    {
        ValueTask RegisterThrottleAsync(string throttleName, int trainAddress, CancellationToken cancellationToken);

        ValueTask RegisterThrottleAsync(string throttleName, string rosterName, CancellationToken cancellationToken);
     
        ValueTask SetThrottleDataAsync(ThrottleData throttleData, CancellationToken cancellationToken);
    }
}