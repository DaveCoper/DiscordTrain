
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public interface IJMRIConnection
    {
        ValueTask ConnectAsync(CancellationToken cancellationToken);

        ValueTask SendAsync(JMRIMessage message);

        ValueTask SendAsync<TMessage>(TMessage message);

        ValueTask<IEnumerable<JMRIMessage>> ReceiveMessagesAsync(byte[] buffer, CancellationToken cancellationToken);        
    }
}