using DiscordTrain.ConnectorBase;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace DiscordTrain.RPiConnector
{
    /// <summary>
    /// Controller for Raspberry Pi that uses GPIO to controll the train.
    /// </summary>
    public class GpioControllerConnector : ControllerConnectorBase
    {
        private readonly GpioControllerConnectorOptions options;

        private readonly ILogger<GpioControllerConnector> logger;

        private GpioPin directionPin;

        private GpioPin pwmPin;

        /// <summary>
        /// Contructor for <see cref="GpioControllerConnector">.
        /// </summary>
        /// <param name="options">Controllers options <see cref="GpioControllerConnectorOptions"> for more info.</param>
        /// <param name="logger">Logger for logging.</param>
        public GpioControllerConnector(IOptions<GpioControllerConnectorOptions> options, ILogger<GpioControllerConnector> logger = null)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? NullLogger<GpioControllerConnector>.Instance;
        }

        /// <summary>
        /// Initializes WiringPi and gpio pins.
        /// </summary>
        public override void Initialize()
        {
            Pi.Init<BootstrapWiringPi>();

            directionPin = (GpioPin)Pi.Gpio[FindPin(options.DirectionPinNumber)];
            directionPin.PinMode = GpioPinDriveMode.Output;

            // make train go foward.
            directionPin.Write(true);

            pwmPin = (GpioPin)Pi.Gpio[FindPin(options.PwmPinNumber)];
            pwmPin.PinMode = GpioPinDriveMode.PwmOutput;
            pwmPin.PwmMode = PwmMode.Balanced;
            pwmPin.PwmClockDivisor = 128;
            pwmPin.PwmRange = 1024;

            base.Initialize();
        }

        /// <summary>
        /// Sets state of direction pin.
        /// </summary>
        /// <param name="trainIsGoingFoward">Value of direction pin.</param>
        protected override void SetDirectionInternal(bool trainIsGoingFoward)
        {
            if (directionPin != null)
            {
                directionPin.Write(trainIsGoingFoward);
            }
        }

        /// <summary>
        /// Sets duty cycle of PWM module.
        /// </summary>
        /// <param name="normalizedDutyCycle">Pwm duty cycle. Valid values are between 0 and 1.</param>
        protected override void SetSpeedInternal(double normalizedDutyCycle)
        {
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
