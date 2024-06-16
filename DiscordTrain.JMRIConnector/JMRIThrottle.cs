using Microsoft.Extensions.Logging;

using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector
{
    public class JMRIThrottle : IJMRIThrottle
    {
        private readonly string name;
        private readonly string roosterEntryName;
        private readonly IJMRIConnection jmriConnection;

        private readonly ILogger<JMRIThrottle> logger;

        public double CurrentSpeedPercent { get; protected set; }

        public TrainDirection CurrentDirection { get; protected set; }

        public string Name => name;

        public string RoosterEntryName => roosterEntryName;

        public JMRIThrottle(
            string name,
            string roosterEntryName,
            IJMRIConnection jmriConnection,
            ILogger<JMRIThrottle> logger)
        {
            this.CurrentSpeedPercent = 0.0;
            this.CurrentDirection = TrainDirection.Unknown;

            this.name = name;
            this.roosterEntryName = roosterEntryName;

            this.jmriConnection = jmriConnection;
            this.logger = logger;
        }

        public async Task SetDirectionAsync(TrainDirection direction)
        {
            if (direction == TrainDirection.Unknown)
            {
                throw new ArgumentOutOfRangeException(nameof(direction), "Train direction can't be set to \"Unknown\"");
            }

            await this.jmriConnection.SendAsync(
                new ThrottleData
                {
                    Name = this.Name,
                    RosterEntry = this.RoosterEntryName,
                    Forward = direction == TrainDirection.Forward,
                });
        }

        public async Task SetSpeedAsync(double speedPercent)
        {
            await this.jmriConnection.SendAsync(new ThrottleData
            {
                Name = this.Name,
                RosterEntry = this.RoosterEntryName,
                Speed = speedPercent * 0.01,
            });
        }

        public async Task RefreshValuesAsync()
        {
            await this.jmriConnection.SendAsync(new ThrottleData
            {
                Name = this.Name,
                RosterEntry = this.RoosterEntryName,
            });
        }

        public void SetThrottleData(ThrottleData trainData)
        {
            if (trainData.Speed.HasValue)
            {
                this.CurrentSpeedPercent = trainData.Speed.Value * 100.0;
            }

            if (trainData.Forward.HasValue)
            {
                this.CurrentDirection = trainData.Forward.Value ?
                    TrainDirection.Forward :
                    TrainDirection.Backward;
            }


        }
    }
}