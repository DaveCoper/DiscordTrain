using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public interface IMessageRouter
    {
        void RegisterThrottle(IJMRIThrottle throttle);

        ValueTask RouteMessage(JMRIMessage message);
    }
}