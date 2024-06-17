using System.Threading.Tasks;

using Discord.Commands;
using System.Text;
using DiscordTrain.Common;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace DiscordTrain.CommandModules
{
    public class TrainRosterModule : ModuleBase<SocketCommandContext>
    {
        private readonly IRosterProvider rosterProvider;

        public TrainRosterModule(IRosterProvider rosterProvider)
        {
            this.rosterProvider = rosterProvider;
        }

        /// <summary>
        /// Displays entire train roster.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        [Command("roster")]
        [Summary("Show train roster.")]
        public async Task ShowRosterAsync()
        {
            var rosterEnumerator = await rosterProvider.GetRosterEntriesAsync(CancellationToken.None);
            var rosterList = rosterEnumerator.ToList();
            string finalText = FormatRoster(rosterList);

            //logger.LogTrace("Displaying roster");

            await ReplyAsync(finalText);
        }

        private string FormatRoster(List<IRosterEntry> rosterList)
        {
            var sb = new StringBuilder();
            if (rosterList.Count == 0)
            {
                sb.Append("No trains connected!");
            }
            else
            {
                sb.AppendLine("Available trains:");
                var format = "{0,-15} {1,20} {2,10}";
                sb.Append("```");
                sb.AppendFormat(format, "Name", "Model", "Number");
                sb.AppendLine();

                foreach (var entry in rosterList)
                {
                    sb.AppendFormat(format,
                        entry.Name.Replace(' ', '_'),
                        entry.Model,
                        entry.Number);
                    sb.AppendLine();
                }
            }

            sb.Append("```");
            var finalText = sb.ToString();

            return finalText;
        }
    }
}