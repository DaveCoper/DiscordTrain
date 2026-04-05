using DiscordTrain.Common.Commands;
using DiscordTrain.RPiConnector.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTrain.RPiConnector.DependencyInjection;

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
}
