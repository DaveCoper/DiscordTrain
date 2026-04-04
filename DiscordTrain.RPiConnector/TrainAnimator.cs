using System;

using DiscordTrain.RPiConnector;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DiscordTrain
{
    public class TrainAnimator : ITrainAnimator
    {
        private readonly RPiConnectorOptions options;

        private double desiredSpeed;

        private int cooldown;

        private bool emergencyStop;

        public TrainAnimator(IGpioTrainController trainController, IOptions<RPiConnectorOptions> options, ILogger<TrainAnimator> logger = null)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.trainController = trainController ?? throw new ArgumentNullException(nameof(trainController));
            this.logger = logger ?? NullLogger<TrainAnimator>.Instance;
        }

        public double DesiredSpeed
        {
            get => desiredSpeed;
            set => desiredSpeed = Math.Clamp(value, -100.0, 100.0);
        }

        public double CurrentSpeed { get; private set; }

        private readonly IGpioTrainController trainController;
        public readonly ILogger<TrainAnimator> logger;

        public void EmergencyStop()
        {
            this.DesiredSpeed = 0;
            this.emergencyStop = true;
            this.Animate();
        }

        public void Animate()
        {
            if (emergencyStop)
            {
                emergencyStop = false;
                this.CurrentSpeed = 0;
                this.cooldown = options.EmergencyStopLenghtInNumberOfTicks;
                return;
            }

            if (this.cooldown > 0)
            {
                --this.cooldown;
                return;
            }

            var currentSpeed = this.CurrentSpeed;
            var nextSpeed = this.CurrentSpeed;
            var targetSpeed = this.DesiredSpeed;

            if (Math.Abs(currentSpeed - targetSpeed) < options.SpeedChangeInTick)
            {
                nextSpeed = targetSpeed;
            }
            else if (currentSpeed > targetSpeed)
            {
                nextSpeed = currentSpeed - options.SpeedChangeInTick;
            }
            else if (currentSpeed < targetSpeed)
            {
                nextSpeed = currentSpeed + options.SpeedChangeInTick;
            }

            if ((currentSpeed > 0 && nextSpeed <= 0) || (currentSpeed < 0 && nextSpeed >= 0))
            {
                nextSpeed = 0;
                cooldown = options.StopLenghtInNumberOfTicks;
            }

            SetSpeed(nextSpeed);
            this.CurrentSpeed = nextSpeed;
        }

        private void SetSpeed(double speed)
        {
            if (Math.Abs(speed) < 1)
            {
                this.trainController.SetSpeed(0);
                return;
            }
            else if (speed < 0)
            {
                this.trainController.SetDirection(Common.TrainDirection.Backward);
            }
            else if (speed > 0)
            {
                this.trainController.SetDirection(Common.TrainDirection.Forward);
            }

            var normalizedSpeed = ((this.options.MaximumSpeedPercent - this.options.MinimumSpeedPercent) * Math.Abs(speed / 100.0) + this.options.MinimumSpeedPercent) / 100.0;
            this.trainController.SetSpeed(normalizedSpeed);
        }

        #region IDisposable implementation

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TrainController()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this.StopAnimation();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        #endregion
    }
}
