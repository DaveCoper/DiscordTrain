using System;

namespace DiscordTrain.ConnectorBase
{
    /// <summary>
    /// Base class for connector that provides input validation.
    /// </summary>
    public abstract class ControllerConnectorBase : IControllerConnector
    {
        private double currentNormalizedDutyCycle;

        private bool currentlyTrainIsGoingFoward;

        /// <inheritdoc cref="IControllerConnector.Initialize"/>
        public virtual void Initialize()
        { }

        /// <inheritdoc cref="IControllerConnector.SetSpeed(double)"/>
        public void SetSpeed(double normalizedDutyCycle)
        {
            if (normalizedDutyCycle < 0 || normalizedDutyCycle > 1)
                throw new ArgumentOutOfRangeException(nameof(normalizedDutyCycle), normalizedDutyCycle, $"{normalizedDutyCycle} must be between 0 to 1");

            if (currentNormalizedDutyCycle != normalizedDutyCycle)
            {
                SetSpeedInternal(normalizedDutyCycle);
                currentNormalizedDutyCycle = normalizedDutyCycle;
            }
        }

        /// <inheritdoc cref="IControllerConnector.SetDirection(bool)"/>
        public void SetDirection(bool trainIsGoingFoward)
        {
            if (currentlyTrainIsGoingFoward != trainIsGoingFoward)
            {
                if (currentNormalizedDutyCycle != 0.0)
                    throw new InvalidOperationException("You can't change direction when train is moving");

                SetDirectionInternal(trainIsGoingFoward);
                currentlyTrainIsGoingFoward = trainIsGoingFoward;
            }
        }

        protected abstract void SetDirectionInternal(bool trainIsGoingFoward);

        protected abstract void SetSpeedInternal(double normalizedDutyCycle);
    }
}
