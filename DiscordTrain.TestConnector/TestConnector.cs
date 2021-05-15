using DiscordTrain.ConnectorBase;
using System;
using System.Threading.Tasks;

namespace DiscordTrain.TestConnector
{
    public class TestConnector : ControllerConnectorBase
    {
        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override Task SetDirection(bool trainIsGoingFoward)
        {
            throw new NotImplementedException();
        }

        protected override Task SetPwmDutyCycleInternal(double normalizedDutyCycle)
        {
            throw new NotImplementedException();
        }
    }
}
