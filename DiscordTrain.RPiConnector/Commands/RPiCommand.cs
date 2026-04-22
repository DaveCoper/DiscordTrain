using DiscordTrain.Common.Commands;

namespace DiscordTrain.RPiConnector.Commands;

public class RPiCommand : IConnectorCommand
{
    public string ConnectorName => "Raspberry Pi";
}