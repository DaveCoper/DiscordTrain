using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;
using System.Xml.Linq;

namespace DiscordTrain.JMRIConnector
{
    public interface IJMRIThrottle : ITrainThrottle
    {
        public string Name { get; }

        public string RoosterEntryName { get; }

        void SetThrottleData(ThrottleData trainData);
    }
}