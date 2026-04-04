using DiscordTrain.Common;

namespace DiscordTrain.RPiConnector
{
    public interface IGpioTrainController
    {
        void Initialize();
        void SetDirection(TrainDirection trainDirection);
        void SetSpeed(double normalizedDutyCycle);
    }
}