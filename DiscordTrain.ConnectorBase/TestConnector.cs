using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DiscordTrain.ConnectorBase
{
    /*
    public class TestConnector : ControllerConnectorBase
    {
        private readonly ILogger<TestConnector> logger;

        public TestConnector(ILogger<TestConnector> logger)
        {
            this.logger = logger ?? NullLogger<TestConnector>.Instance;
        }

        protected override void SetDirectionInternal(bool trainIsGoingFoward)
        {
            this.logger.LogInformation(trainIsGoingFoward ? "Train is going foward." : "Train is going backward.");
        }

        protected override void SetSpeedInternal(double normalizedDutyCycle)
        {
            this.logger.LogInformation("Train speed is {0:N2}.", normalizedDutyCycle);
        }
    }
    */
}