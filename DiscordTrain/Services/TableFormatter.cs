using System.Collections.Generic;
using System.IO;
using System.Linq;

using Spectre.Console;

namespace DiscordTrain.Services;

public class TableFormatter : ITableFormatter
{
    public string FormatData<TRow>(List<TRow> trainRoster)
    {
        var properties = typeof(TRow).GetProperties();
        return FormatData(trainRoster, properties);
    }

    public string FormatData<TRow>(List<TRow> trainRoster, params string[] properties)
    {
        var objectPropeties = typeof(TRow).GetProperties()
            .Join(properties, x => x.Name.ToLower(), x => x.ToLower(), (prop, propName) => prop)
            .ToArray();

        return FormatData(trainRoster, objectPropeties);
    }

    private static string FormatData<TRow>(List<TRow> trainRoster, System.Reflection.PropertyInfo[] properties)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
            Out = new AnsiConsoleOutput(writer)
        });

        var table = new Table();
        foreach (var property in properties)
        {
            table.AddColumn(property.Name);
        }

        foreach (var entry in trainRoster)
        {
            var data = properties.Select(p => p.GetValue(entry)?.ToString() ?? string.Empty).ToArray();
            table.AddRow(data);

        }

        console.Write(table);
        return writer.ToString();
    }
}
