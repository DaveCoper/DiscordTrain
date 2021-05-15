using System;
using System.Threading;
using DiscordTrain.ConnectorBase;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace DiscordTrain
{
    public class TrainAnimator : IDisposable
    {
        private readonly TrainAnimatorOptions options;

        private Timer timer;

        private bool disposedValue;

        private double desiredSpeed;

        private int cooldown;

        private bool emergencyStop;

        public TrainAnimator(IControllerConnector controllerConnector, IOptions<TrainAnimatorOptions> options, ILogger<TrainAnimator> logger = null)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.controllerConnector = controllerConnector ?? throw new ArgumentNullException(nameof(controllerConnector));
            this.logger = logger ?? NullLogger<TrainAnimator>.Instance;
        }

        public double DesiredSpeed
        {
            get => desiredSpeed;
            set => desiredSpeed = Math.Max(-1.0, Math.Min(1.0, value));
        }

        public double CurrentSpeed { get; private set; }

        private readonly IControllerConnector controllerConnector;
        public readonly ILogger<TrainAnimator> logger;

        public void EmergencyStop()
        {
            this.DesiredSpeed = 0;
            this.emergencyStop = true;
        }

        public void StartAnimation()
        {
            if (timer != null)
                return;

            timer = new Timer(this.Animate, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
        }

        public void StopAnimation()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }

            SetPwmCycle(0);
        }

        private void Animate(object state)
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

            if ((currentSpeed > 0 && nextSpeed < 0) || (currentSpeed < 0 && nextSpeed > 0))
            {
                nextSpeed = 0;
                cooldown = options.EmergencyStopLenghtInNumberOfTicks;
            }

            SetSpeed(nextSpeed);
            this.CurrentSpeed = nextSpeed;
        }

        private void SetSpeed(double speed)
        {
            if (Math.Abs(speed) < options.MinimumSpeedPercent)
            {
                speed = 0;
            }
            else if (speed < 0)
            {
                SetDirectionPin(false);
            }
            else if (speed > 0)
            {
                SetDirectionPin(true);
            }

            this.controllerConnector.SetSpeed(speed * 0.01);
        }

        private void SetPwmCycle(double speed)
        {
            logger.LogTrace("PWM duty cycle is set to {0:N2}.", speed);

        }

        private void SetDirectionPin(bool state)
        {
            logger.LogInformation("Train is going {0}.", state ? "Forward" : "Backward");

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
