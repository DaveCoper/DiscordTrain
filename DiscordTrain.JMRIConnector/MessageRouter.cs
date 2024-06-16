using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;
using DiscordTrain.JMRIConnector.WebSocketServices;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiscordTrain.JMRIConnector
{
        /*
    public class MessageRouter : IMessageRouter
    {
        private readonly IJMRIConnection jmriConnection;
        private readonly IHeartbeatService heartbeatService;
        private readonly IRosterProvider rosterStore;

        private readonly ILogger<MessageRouter> logger;

        private readonly ConcurrentBag<IJMRIThrottle> throttles;

        public MessageRouter(
            IJMRIConnection jmriConnection,
            IHeartbeatService heartbeatService,
            IRosterProvider store,
            ILogger<MessageRouter> logger)
        {
            this.jmriConnection = jmriConnection;
            this.heartbeatService = heartbeatService;
            this.rosterStore = store;
            this.logger = logger;

            this.throttles = new ConcurrentBag<IJMRIThrottle>();
        }
        public async ValueTask RouteMessage(JMRIMessage message)
        {
            switch (message.Type)
            {
                case "ping":
                    await this.heartbeatService.SendPong();
                    break;

                case "hello":
                    var helloMsg = (JMRIMessage<HelloData>)message;
                    this.logger.LogInformation("Conected to JMRI railroad: {railroadName}", helloMsg.Data.Railroad);
                    this.SetHeartbeat(helloMsg.Data.Heartbeat);
                    break;

                case "throttle":
                    var throttleMsg = (JMRIMessage<ThrottleData>)message;
                    UpdateThrottleData(throttleMsg.Data);
                    break;

                case "rosterEntry":
                    var rosterMsg = (JMRIMessage<RosterEntryData>)message;
                    this.rosterStore.UpdateRoster(rosterMsg.Data);
                    break;

                case "error":
                    var errorMsg = (JMRIMessage<ErrorData>)message;
                    this.logger.LogError("JMRI reported error: {code} {error}", errorMsg.Data.Code, errorMsg.Data.Message);
                    break;

                default:
                    this.logger.LogWarning("JMRI send something that was ignored: {msgType}", message.Type);
                    break;

            }
        }

        private void UpdateThrottleData(ThrottleData data)
        {
            var activeThrottles = this.throttles
                .Where(x => x.RoosterEntryName == data.RosterEntry)
                .ToList();

            foreach (var throttle in activeThrottles)
            {
                throttle.UpdateThrottleData(data);
            }
        }

        private void SetHeartbeat(double heartbeatInterval)
        {
            this.heartbeatService.SetHeartbeatInterval((int)heartbeatInterval);
        }

        public void RegisterThrottle(IJMRIThrottle throttle)
        {
            this.throttles.Add(throttle);
        }
    }
        */
}