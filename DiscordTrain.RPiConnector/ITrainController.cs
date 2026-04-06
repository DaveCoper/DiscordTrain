using DiscordTrain.Common;

namespace DiscordTrain.RPiConnector
{
    public interface ITrainController
    {
        void Initialize();
        void SetDirection(TrainDirection trainDirection);
        void SetSpeed(double normalizedDutyCycle);
    }
}