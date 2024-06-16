namespace DiscordTrain.JMRIConnector.Messages
{
    public class JMRIMessage
    {
        public string Type { get; set; } = string.Empty;

        public string? Method { get; set; }
        
        public int? Id { get; set; }
    }
}
