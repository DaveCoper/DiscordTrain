using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Commands;

namespace DiscordTrain
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public HelpModule()
        {
        }

        /// <summary>
        /// Sets train speed.
        /// </summary>
        /// <param name="speed">Desired speed.</param>
        /// <returns>Asynchronous task.</returns>        
        [Command("help")]
        [Summary("Prints help.")]
        public async Task PrintHelpAsync()
        {
            var commands = this.GetType().Assembly.GetTypes()
                .Where(x => x.IsAssignableTo(typeof(ModuleBase<SocketCommandContext>)))
                .SelectMany(x => x.GetMethods())
                .Select(x => new { Command = x.GetCustomAttribute<CommandAttribute>(), Method = x })
                .Where(x => x.Command != null)
                .Select(x => new
                {
                    Command = x.Command.Text,
                    Summary = x.Method.GetCustomAttribute<SummaryAttribute>()?.Text,
                    Args = x.Method.GetParameters().Select(p => new { p.Name, p.GetCustomAttribute<SummaryAttribute>()?.Text, p.ParameterType }).ToList()
                })
                .Select(x=> new { 
                    Command = $"!{x.Command} {string.Join(" ", x.Args.Select(a => $"[{a.Name}]"))}".Trim(),
                    Summary = x.Summary,
                    Args = x.Args,
                })
                .ToList();

            var sb = new StringBuilder();

            sb.Append("```");

            var commandColSize = commands.Max(x => x.Command.Length);
            var summaryColSize = commands.Max(x => x.Summary.Length);

            var headerFormat = $"{{0,-{commandColSize}}}\t{{1,-{summaryColSize}}}";
            var paramsFormat = $"{{0,{commandColSize}}}\t{{1,-{summaryColSize}}}";

            foreach (var command in commands)
            {
                sb.AppendFormat(headerFormat, command.Command, command.Summary);
                sb.AppendLine();

                foreach (var arg in command.Args)
                {
                    sb.AppendFormat(paramsFormat, arg.Name, arg.Text);
                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            sb.Append("```");

            var text = sb.ToString();
            await ReplyAsync(text);
        }
    }
}