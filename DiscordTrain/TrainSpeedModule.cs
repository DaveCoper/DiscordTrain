using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DiscordTrain
{
    public class TrainSpeedModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<TrainSpeedModule> logger;

        private readonly TrainAnimator trainController;

        public TrainSpeedModule(TrainAnimator trainController, ILogger<TrainSpeedModule> logger)
        {
            this.trainController = trainController ?? throw new ArgumentNullException(nameof(trainController));
            this.logger = logger ?? NullLogger<TrainSpeedModule>.Instance;
        }

        /// <summary>
        /// Sets train speed.
        /// </summary>
        /// <param name="speed">Desired speed.</param>
        /// <returns>Asynchronous task.</returns>        
        [Command("speed")]
        [Summary("Sets train speed.")]
        public async Task SetSpeedAsync([Summary("Desired train speed. Valid values are between -100 to 100.")] double speed)
        {
            speed = Math.Min(100.0, Math.Max(-100.0, speed));

            logger.LogTrace("Setting train speed to {0}", speed);
            trainController.DesiredSpeed = speed;
            await ReplyAsync($"Setting train speed to {speed}.");
        }

        /// <summary>
        /// EmergencyStop
        /// </summary>
        /// <returns></returns>
        [Command("stop")]
        [Summary("Stops the train.")]
        public async Task StopAsync()
        {
            logger.LogTrace("Emergency stop!");
            trainController.EmergencyStop();
            await ReplyAsync($"Train was stoped.");
        }
    }
}