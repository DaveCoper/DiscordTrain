using DiscordTrain.Common.Commands;
using DiscordTrain.JMRIConnector.Services;
using DiscordTrain.JMRIConnector.WebApiServices;
using DiscordTrain.JMRIConnector.WebSocketServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTrain.JMRIConnector.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterJMRIConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "JMRI")
    {
        // register configuration
        services.Configure<JMRIConnectorOptions>(configuration.GetSection(JMRIConnectorOptions.OptionsKey));

        // register commands
        services.AddKeyedScoped<IListTrainsCommand>(serviceKey, (serviceProvider, key) =>
        {
            var rosterService = serviceProvider.GetRequiredKeyedService<IRosterService>(key);
            return new Commands.ListTrainsCommand(rosterService);
        });

        services.AddKeyedScoped<IListSensorsCommand>(serviceKey, (serviceProvider, key) =>
        {
            var sensorService = serviceProvider.GetRequiredKeyedService<ISensorService>(key);
            return new Commands.ListSensorsCommand(sensorService);
        });

        // register services
        services.AddKeyedScoped<IRosterService>(serviceKey, (serviceProvider, key) =>
        {
            var apiClient = serviceProvider.GetRequiredKeyedService<IJMRIWebApiClient>(key);
            return new RosterService(apiClient);
        });

        services.AddKeyedScoped<ISensorService>(serviceKey, (serviceProvider, key) =>
        {
            var apiClient = serviceProvider.GetRequiredKeyedService<IJMRIWebApiClient>(key);
            return new SensorService(apiClient);
        });

        services.AddKeyedScoped<IJMRIWebApiClient>(serviceKey, (serviceProvider, key) =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient((string)key!);

            return new JMRIWebApiClient(
                httpClient,
                serviceProvider.GetRequiredKeyedService<IMessageSerializer>(key),
                serviceProvider.GetRequiredService<ILogger<JMRIWebApiClient>>()
            );
        });

        services.AddKeyedSingleton<IMessageSerializer, JMRIMessageSerializer>(serviceKey);
        services.AddKeyedSingleton<IJMRIWebSocketClient>(serviceKey, (serviceProvider, key) =>
        {
            return new JMRIWebSocketClient(
                serviceProvider.GetRequiredKeyedService<IMessageSerializer>(key),
                serviceProvider.GetRequiredService<IOptions<JMRIConnectorOptions>>(),
                serviceProvider.GetRequiredService<ILogger<JMRIWebSocketClient>>()
            );
        });

        services.AddHttpClient(serviceKey, (serviceProvider, client) =>
        {
            var jmriOptions = serviceProvider.GetRequiredService<IOptions<JMRIConnectorOptions>>();
            client.BaseAddress = new Uri(jmriOptions.Value.WebServerUrl);
        });

        return services;
    }
}
