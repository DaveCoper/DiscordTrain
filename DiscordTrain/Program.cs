using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordTrain.CommandModules;
using DiscordTrain.JMRIConnector.DependencyInjection;
using DiscordTrain.RPiConnector.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordTrain
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("appsettings.local.json", true, true);

                    builder.AddUserSecrets<Program>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterRPiConnector(hostContext.Configuration);
                    services.RegisterJMRIConnector(hostContext.Configuration);

                    services.AddSingleton(DiscordSocketClientFactory);
                    services.AddSingleton(DiscordCommandServiceFactory);
                })
                .Build();

            await host.RunAsync();
        }

        private static CommandService DiscordCommandServiceFactory(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<CommandService>>();
            var service = new CommandService();
            service.Log += message => LogDiscordMessage(message, logger);

            service.AddModuleAsync<TrainRosterModule>(serviceProvider);
            service.AddModuleAsync<TrainSpeedModule>(serviceProvider);
            service.AddModuleAsync<HelpModule>(serviceProvider);

            return service;
        }

        private static DiscordSocketClient DiscordSocketClientFactory(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DiscordSocketClient>>();

            var config = new DiscordSocketConfig {  };
            config.GatewayIntents = config.GatewayIntents | GatewayIntents.MessageContent;

            var client = new DiscordSocketClient(config);

            client.Log += message => LogDiscordMessage(message, logger);
            return client;
        }

        private static Task LogDiscordMessage(LogMessage message, ILogger logger)
        {
            switch (message.Severity)
            {
                case LogSeverity.Info:
                    logger.LogInformation(message.Exception, message.Message);
                    break;

                case LogSeverity.Warning:
                    logger.LogWarning(message.Exception, message.Message);
                    break;

                case LogSeverity.Debug:
                    logger.LogDebug(message.Exception, message.Message);
                    break;

                case LogSeverity.Verbose:
                    logger.LogTrace(message.Exception, message.Message);
                    break;

                case LogSeverity.Critical:
                    logger.LogCritical(message.Exception, message.Message);
                    break;

                case LogSeverity.Error:
                    logger.LogError(message.Exception, message.Message);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}