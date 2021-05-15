using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DiscordTrain
{
    public class DiscordTrainService : IHostedService
    {
        public const string BotTokenKey = "Discord::BotToken";

        private readonly string botToken;
        private readonly TrainAnimator trainController;
        private readonly CommandService commands;

        private readonly DiscordSocketClient socketClient;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DiscordTrainService> logger;

        public DiscordTrainService(
            IConfiguration configuration,
            TrainAnimator trainController,
            CommandService commands,
            DiscordSocketClient socketClient,
            IServiceProvider serviceProvider,
            ILogger<DiscordTrainService> logger = null)
        {
            botToken = configuration[BotTokenKey];
            if (string.IsNullOrWhiteSpace(botToken))
            {
                throw new InvalidOperationException($"'{nameof(BotTokenKey)}' is missing!");
            }

            this.trainController = trainController ?? throw new ArgumentNullException(nameof(trainController));
            this.commands = commands ?? throw new ArgumentNullException(nameof(commands));
            this.socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.logger = logger ?? NullLogger<DiscordTrainService>.Instance;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting discord train bot.");

            socketClient.MessageReceived += HandleMessageAsync;
            
            this.trainController.StartAnimation();

            await socketClient.LoginAsync(Discord.TokenType.Bot, botToken);
            await socketClient.StartAsync();
        }

        private async Task HandleMessageAsync(SocketMessage arg)
        {
            // Don't process the command if it was a system message
            var message = arg as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(socketClient.CurrentUser, ref argPos)) || 
                message.Author.IsBot || 
                message.Channel.Name != "train-controls" )
            {
                return;
            }

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(socketClient, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: serviceProvider);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Shutting down discord train bot.");
            await this.socketClient.StopAsync();

            this.trainController.StopAnimation();
            this.trainController.Dispose();
        }
    }
}