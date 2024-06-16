namespace DiscordTrain.JMRIConnector.Messages
{
    public class RosterEntryData
    {
        public string? Name { get; set; }

        public int? Address { get; set; }

        public bool? IsLongAddress { get; set; }

        public string? Road { get; set; }

        public string? Number { get; set; }

        public string? Mfg { get; set; }

        public string? DecoderModel { get; set; }

        public string? DecoderFamily { get; set; }

        public string? Model { get; set; }

        public string? Comment { get; set; }

        public int? MaxSpeedPct { get; set; }

        public string? Image { get; set; }
        public string? Icon { get; set; }

        public string? ShuntingFunction { get; set; }

        public string? Owner { get; set; }

        public DateTime? DateModified { get; set; }

        public List<FunctionKey>? FunctionKeys { get; set; }

        /*
        public List<object> Attributes { get; set; } = new List<object>(); // Likely needs further definition

        public List<object> RosterGroups { get; set; } = new List<object>(); // Likely needs further definition
        */
    }

    public class FunctionKey
    {
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool Lockable { get; set; }
        public string? Icon { get; set; } // Can be null, handle accordingly
        public string? SelectedIcon { get; set; } // Can be null, handle accordingly
    }
}
