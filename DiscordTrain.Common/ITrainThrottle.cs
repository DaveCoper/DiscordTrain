namespace DiscordTrain.Common
{
    public interface ITrainThrottle
    {
        double CurrentSpeedPercent { get; }

        TrainDirection CurrentDirection { get; }

        /// <summary>
        /// Sets train speed.
        /// </summary>
        /// <param name="speedPercentage">
        /// Number between 0 and 100 that indicates desired train speed percentage.
        /// Actual train speed is determined by train motor and its settings.
        /// </param>
        /// <returns>Asynchronous task.</returns>
        Task SetSpeedAsync(double speedPercentage);

        /// <summary>
        /// Sets train direction.
        /// </summary>
        /// <param name="direction">Desired train direction.</param>
        /// <returns>Asynchronous task.</returns>
        Task SetDirectionAsync(TrainDirection direction);         
    }
}
