using System;

using DiscordTrain.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace DiscordTrain.RPiConnector
{
    /// <summary>
    /// Controller for Raspberry Pi that uses GPIO to control the train.
    /// </summary>
    public class GpioTrainController : ITrainController
    {
        private readonly RPiConnectorOptions options;

        private readonly ILogger<GpioTrainController> logger;

        private GpioPin directionPin;

        private GpioPin pwmPin;

        /// <summary>
        /// Constructor for <see cref="GpioTrainController">.
        /// </summary>
        /// <param name="options">Controllers options <see cref="GpioControllerConnectorOptions"> for more info.</param>
        /// <param name="logger">Logger for logging.</param>
        public GpioTrainController(IOptions<RPiConnectorOptions> options, ILogger<GpioTrainController> logger = null)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? NullLogger<GpioTrainController>.Instance;
        }

        /// <summary>
        /// Initializes WiringPi and gpio pins.
        /// </summary>
        public void Initialize()
        {
            Pi.Init<BootstrapWiringPi>();

            this.logger.LogInformation("Initializing GPIO train controller with pwm pin: {pwmPin} and direction pin {directionPin}", options.PwmPinNumber, options.DirectionPinNumber);

            var directionPinIndex = FindPin(options.DirectionPinNumber);
            directionPin = (GpioPin)Pi.Gpio[directionPinIndex];
            directionPin.PinMode = GpioPinDriveMode.Output;

            var pwmPinIndex = FindPin(options.PwmPinNumber);
            pwmPin = (GpioPin)Pi.Gpio[pwmPinIndex];
            pwmPin.PinMode = GpioPinDriveMode.PwmOutput;
            pwmPin.PwmMode = PwmMode.Balanced;
            pwmPin.PwmClockDivisor = 128;
            pwmPin.PwmRange = 1024;

            // make train go forward.
            SetSpeed(0);
            SetDirection(TrainDirection.Forward);
        }

        /// <summary>
        /// Sets state of direction pin.
        /// </summary>
        /// <param name="trainIsGoingFoward">Value of direction pin.</param>
        public void SetDirection(TrainDirection trainDirection)
        {
            if (trainDirection == TrainDirection.Unknown)
            {
                logger.LogWarning("Train direction is unknown. Direction pin will not be changed.");
                return;
            }

            if (directionPin != null)
            {
                directionPin.Write(trainDirection == TrainDirection.Backward);
            }
        }

        /// <summary>
        /// Sets duty cycle of PWM module.
        /// </summary>
        /// <param name="normalizedDutyCycle">Pwm duty cycle. Valid values are between 0 and 1.</param>
        public void SetSpeed(double normalizedDutyCycle)
        {
            normalizedDutyCycle = Math.Clamp(normalizedDutyCycle, 0.0, 1.0);

            if (pwmPin != null)
            {
                pwmPin.PwmRegister = (int)(normalizedDutyCycle * pwmPin.PwmRange);
            }
        }

        /// <summary>
        /// Translates <see cref="int"/> into value from <see cref="BcmPin" /> enum.
        /// </summary>
        /// <param name="pinNumber">Bcm pin number.</param>
        /// <returns></returns>
        private BcmPin FindPin(int pinNumber)
        {
            if (pinNumber < 1 || pinNumber > 31)
                throw new ArgumentOutOfRangeException(nameof(pinNumber), pinNumber, $"{nameof(pinNumber)} must be from 1 to 31");

            if (Enum.TryParse<BcmPin>($"Gpio{pinNumber:00}", out var result))
            {
                return result;
            }

            throw new ArgumentException("Failed to parse gpio number!", nameof(pinNumber));
        }
    }
}
