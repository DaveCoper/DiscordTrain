namespace DiscordTrain.Common
{
    public interface IRosterEntry
    {
        public string? Name { get; set; }

        public int? Address { get; set; }

        public bool? IsLongAddress { get; set; }

        public string? Road { get; set; }

        public string? Number { get; set; }

        public string? Model { get; set; }

        public string? Owner { get; set; }
    }
}
