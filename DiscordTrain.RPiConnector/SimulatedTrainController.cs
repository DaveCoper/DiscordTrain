using System;

using DiscordTrain.Common;

using Microsoft.Extensions.Logging;

namespace DiscordTrain.RPiConnector;

public class SimulatedTrainController(ILogger<SimulatedTrainController> logger) : ITrainController
{
    public TrainDirection TrainDirection { get; set; }

    public double TrainSpeed { get; set; }

    public void Initialize()
    {
        logger.LogInformation("Controller initialized.");
    }

    public void SetDirection(TrainDirection trainDirection)
    {
        if (this.TrainDirection != trainDirection)
        {
            if (TrainSpeed > 0)
            {
                logger.LogError("Error: Train changed direction before stoping.");
            }

            this.TrainDirection = trainDirection;
            logger.LogInformation("Train changed direction to {trainDirection}.", trainDirection);
        }
    }

    public void SetSpeed(double normalizedDutyCycle)
    {
        if (Math.Abs(this.TrainSpeed - normalizedDutyCycle) > 0.01)
        {
            this.TrainSpeed = normalizedDutyCycle;
            logger.LogInformation("Train speed changed to {trainSpeed}.", normalizedDutyCycle);
        }
    }
}
