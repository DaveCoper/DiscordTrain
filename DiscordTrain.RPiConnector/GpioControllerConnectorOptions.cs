namespace DiscordTrain.RPiConnector
{
    public class GpioControllerConnectorOptions
    {
        public int PwmPinNumber { get; set; } = 12;

        public int DirectionPinNumber { get; set; } = 1;
    }
}