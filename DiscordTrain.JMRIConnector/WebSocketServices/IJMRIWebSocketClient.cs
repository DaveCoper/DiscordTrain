using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public interface IJMRIWebSocketClient
    {
        ValueTask InitializeAsync(CancellationToken cancellationToken);

        ValueTask SendAsync(JMRIMessage message, CancellationToken cancellationToken);
        
        ValueTask<TResponse> SendAsync<TResponse>(JMRIMessage message, CancellationToken cancellationToken);

        ValueTask ProcessMessagesAsync(byte[] buffer, CancellationToken cancellationToken);

        void AddWeakMessageLisener(Action<object> lisener);
    }
}
