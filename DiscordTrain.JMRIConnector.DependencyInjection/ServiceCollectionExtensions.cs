using DiscordTrain.Common.Commands;
using DiscordTrain.JMRIConnector.WebApiServices;
using DiscordTrain.JMRIConnector.WebSocketServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTrain.JMRIConnector.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterJMRIConnector(this IServiceCollection services, IConfiguration configuration, string serviceKey = "JMRI")
    {
        // register configuration
        services.Configure<JMRIConnectorOptions>(configuration.GetSection(JMRIConnectorOptions.OptionsKey));

        // register commands
        services.AddKeyedScoped<IListRosterCommand, Commands.ListRosterCommand>(serviceKey);
        services.AddKeyedScoped<IRosterService, RosterService>(serviceKey);
        services.AddKeyedScoped<IJMRIWebApiClient, JMRIWebApiClient>(serviceKey);



        services.AddKeyedScoped<IJMRIWebSocketClient, JMRIWebSocketClient>(serviceKey);


        return services;
    }
}
