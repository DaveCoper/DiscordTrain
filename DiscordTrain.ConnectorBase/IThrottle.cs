using System.Threading.Tasks;

namespace DiscordTrain.ConnectorBase
{
    /// <summary>
    /// Interface for interacting with hardware.
    /// </summary>
    public interface IThrottle
    {
        /// <summary>
        /// Intialize controller.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets speed of the train.
        /// </summary>
        /// <param name="normalizedTrainSpeed">Train speed. Valid values are between -1 and 1.</param>
        Task SetSpeed(double normalizedTrainSpeed);
    }
}
