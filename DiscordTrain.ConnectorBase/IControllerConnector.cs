namespace DiscordTrain.ConnectorBase
{
    /// <summary>
    /// Interface for interacting with hardware.
    /// </summary>
    public interface IControllerConnector
    {
        /// <summary>
        /// Intialize controller.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets train direction. Direction can only be changed when train is not moving.
        /// </summary>
        /// <param name="trainIsGoingFoward">Set to true to move foward, False to move in oposite direction.</param>
        void SetDirection(bool trainIsGoingFoward);

        /// <summary>
        /// Sets speed of the train.
        /// </summary>
        /// <param name="normalizedTrainSpeed">Train speed. Valid values are between 0 and 1.</param>
        void SetSpeed(double normalizedTrainSpeed);
    }
}
