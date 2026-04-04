using DiscordTrain;
using DiscordTrain.RPiConnector;

using Microsoft.Extensions.Options;

namespace BlazorTrain;

public class RPiAnimatorService(
    ITrainAnimator trainAnimator, 
    IGpioTrainController gpioTrainController, 
    IOptions<RPiConnectorOptions> options, 
    ILogger<RPiAnimatorService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting RPi animator service with options: {@options}", options.Value);
        gpioTrainController.Initialize();

        while (!stoppingToken.IsCancellationRequested)
        {
            trainAnimator.Animate();
            await Task.Delay(TimeSpan.FromMilliseconds(options.Value.AnimatorUpdateIntervalInMs), stoppingToken);
        }

        logger.LogInformation("Stopping RPi animator service.");
    }
}