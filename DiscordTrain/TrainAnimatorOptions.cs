namespace DiscordTrain
{
    public class TrainAnimatorOptions
    {
        public double MaximumSpeedPercent { get; set; } = 100;

        public double MinimumSpeedPercent { get; set; } = 25;

        public double SpeedChangeInTick { get; set; } = 5;

        public double AnimatorUpdateIntervalInMs { get; set; } = 250;

        public int EmergencyStopLenghtInNumberOfTicks { get; set; } = 20;

        public int StopLenghtInNumberOfTicks { get; set; } = 4;
    }
}
