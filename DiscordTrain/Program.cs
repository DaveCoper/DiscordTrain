using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordTrain.ConnectorBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#if !TestConnector
using DiscordTrain.RPiConnector;
using Microsoft.Extensions.Options;
#endif

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
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions<TrainAnimatorOptions>();
                    services.AddHostedService<DiscordTrainService>();

                    services.AddSingleton<TrainAnimator>();
#if TestConnector
                    services.AddSingleton<IControllerConnector, TestConnector>(services =>
                    {
                        var connector = new TestConnector(services.GetRequiredService<ILogger<TestConnector>>());
                        connector.Initialize();
                        return connector;
                    });
#else
                    services.AddSingleton<IControllerConnector, GpioControllerConnector>(services =>
                    {
                        var options = services.GetRequiredService<IOptions<GpioControllerConnectorOptions>>();
                        var logger = services.GetRequiredService<ILogger<GpioControllerConnector>>();
                        var controller = new GpioControllerConnector(options, logger);
                        controller.Initialize();
                        return controller;
                    });
#endif

                    services.AddSingleton(CreateDiscordSocketClient);
                    services.AddSingleton(CreateDiscordCommandService);
                })
                .Build();

            await host.RunAsync();
        }

        private static CommandService CreateDiscordCommandService(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<CommandService>>();
            var service = new CommandService();
            service.Log += message => LogDiscordMessage(message, logger);
            service.AddModuleAsync<TrainSpeedModule>(serviceProvider);
            return service;
        }

        private static DiscordSocketClient CreateDiscordSocketClient(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DiscordSocketClient>>();
            var client = new DiscordSocketClient();
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