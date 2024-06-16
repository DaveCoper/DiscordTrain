using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public interface IJMRIWebSocketClient : IJMRIApiClient
    {
        ValueTask InitializeAsync(CancellationToken cancellationToken);

        void AddWeakMessageLisener(Func<JMRIMessage, Task> lisener);
    }
}
