using DiscordTrain.Common.Commands;
using DiscordTrain.RPiConnector.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTrain.RPiConnector.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRPiConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "RaspberryPi")
    {
        services.RegisterCommonComponents(configuration, serviceKey);
        return services.AddKeyedSingleton<ITrainController, GpioTrainController>(serviceKey);
    }

    public static IServiceCollection RegisterSimulatedRPiConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "RaspberryPi")
    {
        services.RegisterCommonComponents(configuration, serviceKey);
        return services.AddKeyedSingleton<ITrainController, SimulatedTrainController>(serviceKey);
    }

    internal static IServiceCollection RegisterCommonComponents(this IServiceCollection services, IConfiguration configuration, string serviceKey) {
        // register configuration
        services.Configure<RPiConnectorOptions>(configuration.GetSection(RPiConnectorOptions.OptionsKey));

        // register services
        services.AddKeyedSingleton<ITrainAnimator>(serviceKey, (serviceProvider, key) =>
        {
            return new TrainAnimator(
                serviceProvider.GetRequiredKeyedService<ITrainController>(key),
                serviceProvider.GetRequiredService<IOptions<RPiConnectorOptions>>(),
                serviceProvider.GetRequiredService<ILogger<TrainAnimator>>());
        });

        services.AddHostedService((serviceProvider) =>
        {
            return new RPiAnimatorService(
                serviceProvider.GetRequiredKeyedService<ITrainAnimator>(serviceKey),
                serviceProvider.GetRequiredKeyedService<ITrainController>(serviceKey),
                serviceProvider.GetRequiredService<IOptions<RPiConnectorOptions>>(),
                serviceProvider.GetRequiredService<ILogger<RPiAnimatorService>>());
        });

        // register commands
        services.AddKeyedSingleton<IListRosterCommand, ListRosterCommand>(serviceKey);
        return services;
    }
}
