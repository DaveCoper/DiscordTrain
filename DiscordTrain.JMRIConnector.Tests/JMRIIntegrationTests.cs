using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DiscordTrain.JMRIConnector.WebApiServices;
using DiscordTrain.JMRIConnector.Services;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.WebSocketServices;

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
        public async Task TestConnection()
        {
            var throttleName = "TestThrottle";
            var tokenSource = new CancellationTokenSource();
            var httpClient = new HttpClient();
            var serverOptions = Options.Create(new JMRIOptions { JMRIWebServerUrl = "http://localhost:12080" });
            var serializer = new JMRIMessageSerializer();
            var loggerFactory = new NullLoggerFactory();

            var connection = new JMRIWebApiClient(
                httpClient,
                serializer,
                serverOptions,
                loggerFactory.CreateLogger<JMRIWebApiClient>());

            var rosterService = new RosterService(connection);
            var roster = await rosterService.GetRosterEntriesAsync(tokenSource.Token);

            var firstRosterEntry = roster.FirstOrDefault();
            if (firstRosterEntry == null)
            {
                Assert.Fail("No roster entries found!");
                return;
            }

            var websocket = new JMRIWebSocketClient(
                serializer,
                serverOptions,
                loggerFactory.CreateLogger<JMRIWebSocketClient>());
            
            await websocket.InitializeAsync(tokenSource.Token);
            var processingTask = new Thread(() => {
                var buffer = new byte[20000];

                while (!tokenSource.IsCancellationRequested)
                {
                    websocket.ProcessMessagesAsync(buffer, tokenSource.Token)
                        .Wait();
                }
            });
            processingTask.Start();

            var throttleService = new ThrottleService(websocket);

            ThrottleData throttleData;
            if (!string.IsNullOrEmpty(firstRosterEntry.Name))
            {
                throttleData = await throttleService.GetThrottleDataAsync(throttleName, firstRosterEntry.Name, tokenSource.Token);
            }
            else if (firstRosterEntry.Address.HasValue)
            {
                throttleData = await throttleService.GetThrottleDataAsync(throttleName, firstRosterEntry.Address.Value, tokenSource.Token);
            }
            else
            {
                Assert.Fail("Can identify train in roster entry!");
                return;
            }

            var throttle = new JMRIThrottle(throttleName, throttleService, loggerFactory.CreateLogger<JMRIThrottle>());
            await throttle.SetSpeedAsync(100);

            Assert.That(throttle.CurrentSpeedPercent, Is.EqualTo(100));
            tokenSource.Cancel();
        }
    }
}