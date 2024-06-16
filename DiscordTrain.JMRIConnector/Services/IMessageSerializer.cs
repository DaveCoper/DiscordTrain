using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Services
{
    public interface IMessageSerializer
    {
        string Serialize<TMessage>(TMessage message);


        TOut? Deserialize<TOut>(string json);
    }
}