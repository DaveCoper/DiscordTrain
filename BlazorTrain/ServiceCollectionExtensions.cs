using DiscordTrain;
using DiscordTrain.RPiConnector;

namespace BlazorTrain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRPiConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "RaspberryPi")
    {
        services.Configure<RPiConnectorOptions>(configuration.GetSection(RPiConnectorOptions.OptionsKey));
        services.AddKeyedSingleton<IGpioTrainController, GpioTrainController>(serviceKey);
        services.AddKeyedSingleton<ITrainAnimator, TrainAnimator>(serviceKey);
        services.AddHostedService<RPiAnimatorService>();

        return services;
    }
    
    public static IServiceCollection RegisterJMRIConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "JMRI")
    {
        services.Configure<JMRIConnectorOptions>(configuration.GetSection(JMRIConnectorOptions.OptionsKey));
        services.AddKeyedSingleton<IGpioTrainController, GpioTrainController>(serviceKey);
        services.AddKeyedSingleton<ITrainAnimator, TrainAnimator>(serviceKey);
        services.AddHostedService<RPiAnimatorService>();

        return services;
    }


}
