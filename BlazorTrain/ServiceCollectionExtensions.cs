using DiscordTrain;
using DiscordTrain.Common.Commands;
using DiscordTrain.JMRIConnector;
using DiscordTrain.JMRIConnector.Commands;
using DiscordTrain.RPiConnector;

namespace BlazorTrain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRPiConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "RaspberryPi")
    {
        // register configuration
        services.Configure<RPiConnectorOptions>(configuration.GetSection(RPiConnectorOptions.OptionsKey));

        // register services
        services.AddKeyedSingleton<IGpioTrainController, GpioTrainController>(serviceKey);
        services.AddKeyedSingleton<ITrainAnimator, TrainAnimator>(serviceKey);
        services.AddHostedService<RPiAnimatorService>();

        // register commands
        services.AddKeyedSingleton<IListRosterCommand, ListRosterCommand>(serviceKey);
        
        return services;
    }
    
    public static IServiceCollection RegisterJMRIConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "JMRI")
    {
        // register configuration
        services.Configure<JMRIConnectorOptions>(configuration.GetSection(JMRIConnectorOptions.OptionsKey));

        // register commands
        services.AddKeyedSingleton<IListRosterCommand, DiscordTrain.JMRIConnector.Commands.ListRosterCommand>(serviceKey);


        return services;
    }


}
