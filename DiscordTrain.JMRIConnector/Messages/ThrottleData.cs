namespace DiscordTrain.JMRIConnector.Messages
{
    public class ThrottleData
    {        
        public string Name { get; set; } = string.Empty;

        public string? RosterEntry { get; set; }

        public int? Address { get; set; }
        
        public double? Speed { get; set; }

        public bool? Forward { get; set; }
       
        public int? SpeedSteps { get; set; }
        
        public int? Clients { get; set; }
        
        public string? Throttle { get; set; }
    }
}
