namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public interface IHeartbeatService
    {
        ValueTask SendPong();

        ValueTask SendPing();

        void SetHeartbeatInterval(int heartbeatInterval);
    }
}