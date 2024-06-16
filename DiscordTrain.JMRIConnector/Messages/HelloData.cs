namespace DiscordTrain.JMRIConnector.Messages
{
    public class HelloData
    {
        public string JMRI { get; set; } = null!;
        public string Json { get; set; } = null!;
        public string Version { get; set; } = null!;
        public double Heartbeat { get; set; }
        public string Railroad { get; set; } = null!;
        public string Node { get; set; } = null!;
        public string ActiveProfile { get; set; } = null!;
    }
}
