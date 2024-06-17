using DiscordTrain.Common;
using DiscordTrain.JMRIConnector;
using DiscordTrain.JMRIConnector.Services;
using DiscordTrain.JMRIConnector.WebApiServices;
using DiscordTrain.JMRIConnector.WebSocketServices;
using DiscordTrain.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordTrain.Configuration
{
    public static class JMRIExtensions
    {
        public static IServiceCollection AddJMRIServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IJMRIWebSocketClient, JMRIWebSocketClient>();
            services.AddSingleton<IJMRIWebApiClient, JMRIWebApiClient>();

            services.AddSingleton<IThrottleManager, ThrottleManager>();
            services.AddSingleton<IThrottleService, ThrottleService>();

            services.AddSingleton<IRosterProvider, RosterService>();
            services.AddSingleton<IMessageSerializer, JMRIMessageSerializer>();

            services.AddHostedService<JMRIWorkerService>();

            return services;
        }
    }
}
