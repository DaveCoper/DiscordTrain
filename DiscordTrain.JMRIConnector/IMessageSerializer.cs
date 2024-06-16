using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public interface IMessageSerializer
    {
        byte[] Serialize(JMRIMessage message);

        byte[] Serialize<TMessage>(TMessage? message);

        IEnumerable<JMRIMessage> Deserialize(string json);
    }
}