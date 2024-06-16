using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public interface IJMRIThrottle : ITrainThrottle
    {
        public string Name { get; }

        void UpdateThrottleData(ThrottleData trainData);
    }
}