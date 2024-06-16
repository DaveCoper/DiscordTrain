namespace DiscordTrain.JMRIConnector.Messages
{
    public class JMRIMessage<TData> : JMRIMessage
    {
        public TData Data { get; set; } = default!;
    }
}
