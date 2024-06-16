namespace DiscordTrain.JMRIConnector
{
    public interface IHeartbeatService
    {
        ValueTask SendPong();

        ValueTask SendPing();

        void SetHeartbeatInterval(int heartbeatInterval);
    }
}