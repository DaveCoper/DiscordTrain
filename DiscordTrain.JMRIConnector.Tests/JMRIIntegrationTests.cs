using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;
using Microsoft.Extensions.Options;

namespace DiscordTrain.JMRIConnector.Tests
{
    [TestFixture(Explicit = true)]
    public class JMRIIntegrationTests
    {
        /// <summary>
        /// Test change of direction.
        /// </summary>
        /// <param name="trainDirection"></param>
        /// <returns></returns>
        [Test]
        public async Task ChangeSpeed()
        {
            var tokenSource = new CancellationTokenSource();
            var connection = new JMRIConnection(
                new MessageSerializer(),
                Options.Create(new JMRIConnectionOptions { JMRIWebServerUrl = "ws://localhost:12080/json/" }),
                NullLogger<JMRIConnection>.Instance);

            var buffer = new byte[10000];
            await connection.ConnectAsync(tokenSource.Token);
            var messages = await connection.ReceiveMessagesAsync(buffer, tokenSource.Token);

            await connection.SendAsync<RosterData>(null, tokenSource.Token);

            var rosterMessage = messages.OfType<JMRIMessage<RosterEntryData>>().FirstOrDefault();
            while (rosterMessage == null)
            {
                messages = await connection.ReceiveMessagesAsync(buffer, tokenSource.Token); 
                rosterMessage = messages.OfType<JMRIMessage<RosterEntryData>>().FirstOrDefault();
            }

            var throttle = new JMRIThrottle(
                "test",
                rosterMessage.Data.Name,
                connection,
                NullLogger<JMRIThrottle>.Instance);

            await throttle.RefreshValuesAsync();

            var throttleData = messages.OfType<JMRIMessage<ThrottleData>>().FirstOrDefault();
            while (throttleData == null)
            {
                messages = await connection.ReceiveMessagesAsync(buffer, tokenSource.Token);
                throttleData = messages.OfType<JMRIMessage<ThrottleData>>().FirstOrDefault();
            }

            throttle.SetThrottleData(throttleData.Data);

        }
    }
}