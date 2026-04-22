using System.Collections.Generic;

namespace DiscordTrain.Services;

public interface ITableFormatter
{
    string FormatData<TRow>(List<TRow> trainRoster);
}