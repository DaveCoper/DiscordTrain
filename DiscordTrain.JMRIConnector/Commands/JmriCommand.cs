using DiscordTrain.Common.Commands;

namespace DiscordTrain.JMRIConnector.Commands;

public class JmriCommand : IConnectorCommand
{
    public string ConnectorName => "JMRI";
}