namespace DiscordTrain.RPiConnector
{
    public class RPiConnectorOptions
    {
        public const string OptionsKey = "RPiConnector";

        public string TrainId { get; set; } = "DC";

        public string TrainName { get; set; } = "Train";

        /// <summary>
        /// GPIO number used to generate PWM signal that dictates train speed.
        /// </summary>
        public int PwmPinNumber { get; set; } = 12;

        /// <summary>
        /// GPIO number used to set H-Bridge polarity that dictates train direction.
        /// </summary>
        public int DirectionPinNumber { get; set; } = 1;


        public double MaximumSpeedPercent { get; set; } = 100;

        public double MinimumSpeedPercent { get; set; } = 25;

        public double SpeedChangeInTick { get; set; } = 5;

        public double AnimatorUpdateIntervalInMs { get; set; } = 250;

        public int EmergencyStopLenghtInNumberOfTicks { get; set; } = 40;

        public int StopLenghtInNumberOfTicks { get; set; } = 10;
    }
}
