using System.Net.WebSockets;
using System.Text;
using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordTrain.JMRIConnector
{
    /*
    public class JMRIConnection : IJMRIConnection
    {
        private readonly JMRIOptions options;
        private readonly ClientWebSocket webSocket;

        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<JMRIConnection> logger;

        public static readonly EventId FailedToSerializeMsgId = new EventId(10001, "Failed to serialize the message");
        public static readonly EventId FailedToSendMsgId = new EventId(10002, "Failed to send the message");

        public JMRIConnection(
            IMessageSerializer messageSerializer,
            IOptions<JMRIOptions> options,
            ILogger<JMRIConnection> logger)
        {
            this.options = options.Value;
            this.messageSerializer = messageSerializer;
            this.logger = logger;

            this.webSocket = new ClientWebSocket();
        }

        #region connection
        public async ValueTask ConnectAsync(CancellationToken cancellationToken)
        {
            string address = GetWebsocketAddress();
            var uri = new Uri(address);
            await webSocket.ConnectAsync(
                uri,
                cancellationToken);
        }

        private string GetWebsocketAddress()
        {
            string address = options.JMRIWebServerUrl;
            string end = address.EndsWith("/") ? "json" : "/json";

            if (address.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                address = $"ws{options.JMRIWebServerUrl.Substring(5)}{end}";
            }
            else if (address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                address = $"ws{options.JMRIWebServerUrl.Substring(4)}{end}";
            }

            return address;
        }
        #endregion

        #region sending
        public async ValueTask SendAsync(JMRIMessage message)
        {
            await SendAsync(message, CancellationToken.None);
        }

        public async ValueTask SendAsync<TMessage>(TMessage message)
        {
            await this.SendAsync(message, CancellationToken.None);
        }

        public async ValueTask SendAsync(JMRIMessage message, CancellationToken cancellationToken)
        {
            byte[] messageBytes = messageSerializer.Serialize(message);
            await SendAsync(messageBytes, cancellationToken);
        }

        public async Task SendAsync<TMessage>(TMessage? message, CancellationToken cancellationToken)
        {
            byte[] messageBytes = messageSerializer.Serialize(message);
            await SendAsync(messageBytes, cancellationToken);
        }

        public async Task<JMRIMessage<TOutput>> GetAsync<TOutput>(string messageType, CancellationToken cancellationToken)
        {
            return await GetInternalAsync(new JMRIMessage { Type = messageType }, cancellationToken);
        }

        public async Task<JMRIMessage<TOutput>> GetAsync<TInput, TOutput>(TInput input, CancellationToken cancellationToken)
        {

        }

        private async Task<JMRIMessage<TOutput>> GetInternalAsync<TInput, TOutput>(JMRIMessage input, CancellationToken cancellationToken)
        {

        }

        private async Task SendAsync(byte[] messageBytes, CancellationToken cancellationToken)
        {
            try
            {
                await webSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.LogError(FailedToSendMsgId, ex, "Failed to send the message.");
                throw new InvalidOperationException("Failed to send the message.", ex);
            }
        }
        #endregion

        public async ValueTask<IEnumerable<JMRIMessage>> ReceiveMessagesAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            var result = await webSocket.ReceiveAsync(buffer, cancellationToken);
            if (result.EndOfMessage)
            {
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                return this.messageSerializer.Deserialize(json);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
    */
}