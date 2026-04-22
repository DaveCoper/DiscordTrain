using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Commands;

using DiscordTrain.Common.Model;
using DiscordTrain.Services;

using Spectre.Console;

namespace DiscordTrain.CommandModules;

public class TrainRosterModule : ModuleBase<SocketCommandContext>
{
    private readonly ITrainRosterProvider trainRosterProvider;
    private readonly ITableFormatter tableFormatter;

    public TrainRosterModule(
        ITrainRosterProvider trainRosterProvider,
        ITableFormatter tableFormatter)
    {
        this.trainRosterProvider = trainRosterProvider;
        this.tableFormatter = tableFormatter;
    }

    /// <summary>
    /// Displays entire train roster.
    /// </summary>
    /// <returns>Asynchronous task.</returns>
    [Command("roster")]
    [Summary("Show train roster.")]
    public async Task ShowRosterAsync()
    {
        var rosterEnumerator = await trainRosterProvider.GetTrainRosterAsync(CancellationToken.None);
        var rosterList = rosterEnumerator.ToList();
        string finalText = this.tableFormatter.FormatData(rosterList);

        //logger.LogTrace("Displaying roster");

        await ReplyAsync(finalText);
    }
}