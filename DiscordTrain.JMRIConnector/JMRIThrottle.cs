﻿using Microsoft.Extensions.Logging;

using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;

namespace DiscordTrain.JMRIConnector
{
    public class JMRIThrottle : IJMRIThrottle
    {
        private readonly string name;
        private readonly IThrottleService jmriConnection;

        private readonly ILogger<JMRIThrottle> logger;

        public double CurrentSpeedPercent { get; protected set; }

        public TrainDirection CurrentDirection { get; protected set; }

        public string Name => name;

        public JMRIThrottle(
            string name,
            IThrottleService jmriConnection,
            ILogger<JMRIThrottle> logger)
        {
            CurrentSpeedPercent = 0.0;
            CurrentDirection = TrainDirection.Unknown;

            this.name = name;

            this.jmriConnection = jmriConnection;
            this.logger = logger;
        }

        public async Task SetDirectionAsync(TrainDirection direction)
        {
            if (direction == TrainDirection.Unknown)
            {
                throw new ArgumentOutOfRangeException(nameof(direction), "Train direction can't be set to \"Unknown\"");
            }

            await jmriConnection.SetThrottleDataAsync(
                new ThrottleData
                {
                    Name = Name,
                    Forward = direction == TrainDirection.Forward,
                }, CancellationToken.None);
        }

        public async Task SetSpeedAsync(double speedPercent)
        {
            await jmriConnection.SetThrottleDataAsync(new ThrottleData
            {
                Name = Name,
                Speed = speedPercent * 0.01,
            }, CancellationToken.None);
        }

        public async Task RefreshValuesAsync()
        {
            await jmriConnection.SetThrottleDataAsync(new ThrottleData
            {
                Name = Name,
            }, CancellationToken.None);
        }

        public void UpdateThrottleData(ThrottleData trainData)
        {
            if (trainData.Speed.HasValue)
            {
                CurrentSpeedPercent = trainData.Speed.Value * 100.0;
            }

            if (trainData.Forward.HasValue)
            {
                CurrentDirection = trainData.Forward.Value ?
                    TrainDirection.Forward :
                    TrainDirection.Backward;
            }
        }
    }
}