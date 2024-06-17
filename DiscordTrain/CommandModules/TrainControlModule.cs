using System;
using System.Threading;
using System.Threading.Tasks;

using Discord.Commands;
using DiscordTrain.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DiscordTrain.CommandModules
{
    public class TrainSpeedModule : ModuleBase<SocketCommandContext>
    {
        private readonly IThrottleManager throttleManager;

        public TrainSpeedModule(IThrottleManager throttleManager)
        {
            this.throttleManager = throttleManager;
        }

        /// <summary>
        /// Sets train speed.
        /// </summary>
        /// <param name="speed">Desired speed.</param>
        /// <returns>Asynchronous task.</returns>        
        [Command("set")]
        [Summary("Sets train speed.")]
        public async Task SetValueAsync(
            [Summary("Train name. You can find it in roster.")] string trainName,
            [Summary("Name of value to be set.")] string valueName,
            [Summary("Value to set.")] string value)
        {
            //speed = Math.Min(100.0, Math.Max(0.0, speed));

            //var throttle = await throttleManager.GetThrottleAsync(trainName, CancellationToken.None);
            //await throttle.SetSpeedAsync(speed);
            //await Task.Delay(200);
            //await ReplyAsync($"Setting train speed to {throttle.CurrentSpeedPercent}.");
        }

        /// <summary>
        /// EmergencyStop
        /// </summary>
        /// <returns></returns>
        [Command("stop")]
        [Summary("Stops the trains.")]
        public async Task StopAsync()
        {
            //logger.LogTrace("Emergency stop!");
            //trainController.EmergencyStop();
            await ReplyAsync($"Train was stoped.");
        }
    }
}