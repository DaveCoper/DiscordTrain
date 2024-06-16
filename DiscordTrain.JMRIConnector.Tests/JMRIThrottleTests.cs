using Microsoft.Extensions.Logging.Abstractions;

using NSubstitute;

using DiscordTrain.Common;
using DiscordTrain.JMRIConnector.Messages;

namespace DiscordTrain.JMRIConnector.Tests
{
    [TestFixture]
    public class JMRIThrottleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test change of direction.
        /// </summary>
        /// <param name="trainDirection"></param>
        /// <returns></returns>
        [Test]
        [TestCase(TrainDirection.Forward)]
        [TestCase(TrainDirection.Backward)]
        public async Task ChangeDirection(TrainDirection trainDirection)
        {
            bool? forward = null;
            var connection = Substitute.For<IJMRIConnection>();
            connection.SendAsync(Arg.Any<ThrottleData>())
                .ReturnsForAnyArgs(ValueTask.CompletedTask)
                .AndDoes(call => { forward = call.ArgAt<ThrottleData>(0).Forward; });

            var connector = new JMRIThrottle(
                "test",
                "test train",
                connection,
                NullLogger<JMRIThrottle>.Instance);

            await connector.SetDirectionAsync(trainDirection);
            connector.SetThrottleData(new ThrottleData { Forward = forward });

            Assert.That(trainDirection, Is.EqualTo(connector.CurrentDirection));
        }

        /// <summary>
        /// Test change of speed.
        /// </summary>
        /// <param name="trainSpeed">train speed</param>
        /// <returns></returns>
        [Test]
        [TestCase(0.0)]
        [TestCase(10.0)]
        [TestCase(25.0)]
        [TestCase(50.0)]
        [TestCase(75.0)]
        [TestCase(100.0)]
        public async Task ChangeSpeed(double trainSpeed)
        {
            var speed = 0.0;
            var connection = Substitute.For<IJMRIConnection>();
            connection.SendAsync(Arg.Any<ThrottleData>())
                .ReturnsForAnyArgs(ValueTask.CompletedTask)
                .AndDoes(call => { speed = call.ArgAt<ThrottleData>(0).Speed ?? -1.0; });

            var connector = new JMRIThrottle(
                "test",
                "test train",
                connection,
                NullLogger<JMRIThrottle>.Instance);

            await connector.SetSpeedAsync(trainSpeed);
            connector.SetThrottleData(new ThrottleData { Speed = speed });

            Assert.That(trainSpeed, Is.EqualTo(connector.CurrentSpeedPercent));
        }
    }
}