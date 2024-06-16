using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Linq;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public class JMRIWebSocketClient : IJMRIWebSocketClient
    {
        private readonly JMRIOptions options;
        private readonly ClientWebSocket webSocket;

        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<JMRIWebSocketClient> logger;

        private readonly ConcurrentDictionary<string, Action<string>> responseDict;

        private int timeoutMs = 30000;

        private int messageCounter = 0;

        public JMRIWebSocketClient(
            IMessageSerializer messageSerializer,
            IOptions<JMRIOptions> options,
            ILogger<JMRIWebSocketClient> logger)
        {
            this.options = options.Value;
            this.messageSerializer = messageSerializer;
            this.logger = logger;

            this.webSocket = new ClientWebSocket();
            this.responseDict = new ConcurrentDictionary<string, Action<string>>();
        }


        public async ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            string address = GetWebsocketAddress();
            var uri = new Uri(address);
            await webSocket.ConnectAsync(
                uri,
                cancellationToken);
        }

        public async ValueTask<TOut?> GetAsync<TOut>(string address, CancellationToken cancellationToken)
        {
            var message = new JMRIMessage { Type = address, Method = "get" };
            return await SendMessage<TOut>(message, cancellationToken);
        }

        public async ValueTask<TOut?> PostAsync<TIn, TOut>(string address, TIn content, CancellationToken cancellationToken)
        {
            var message = content as JMRIMessage ?? new JMRIMessage<TIn> { Data = content, Type = address, Method = "post" };
            return await SendMessage<TOut>(message, cancellationToken);
        }

        public void AddWeakMessageLisener(Func<JMRIMessage, Task> lisener)
        {
            throw new NotImplementedException();
        }

        private async Task<TOut> SendMessage<TOut>(JMRIMessage message, CancellationToken cancellationToken)
        {
            var messageNumber = Interlocked.Increment(ref this.messageCounter);
            var messageId = messageNumber.ToString();
            message.Id = messageNumber;

            if(message is JMRIMessage<ThrottleData> throttleMsg)
            {
                messageId = throttleMsg.Data.Throttle ?? throttleMsg.Data.Name; 
            }

            var json = messageSerializer.Serialize(message);
            Debug.WriteLine($"Sending: {json}");
            var bytes = Encoding.UTF8.GetBytes(json);

            TaskCompletionSource<TOut> taskCompletionSource = new TaskCompletionSource<TOut>();

            var timeout = new CancellationTokenSource(timeoutMs);
            timeout.Token.Register(() => taskCompletionSource.TrySetCanceled(), useSynchronizationContext: false);
            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(), useSynchronizationContext: false);

            this.responseDict.TryAdd(messageId, (response) =>
            {
                var data = messageSerializer.Deserialize<TOut>(response);
                taskCompletionSource.TrySetResult(data ?? throw new NotImplementedException());
            });

            await this.webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
            return await taskCompletionSource.Task;
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

        public async Task ProcessMessagesAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            var result = await webSocket.ReceiveAsync(buffer, cancellationToken);
            if (result.EndOfMessage)
            {
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Debug.WriteLine($"Received: {json}");
                HandleMessagePayload(json);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void HandleMessagePayload(string json)
        {
            var data = messageSerializer.Deserialize<JMRIMessage>(json);
            if (data == null)
                return;

            if (data.Id.HasValue && this.responseDict.TryRemove(data.Id.Value.ToString(), out var responseAction))
            {
                responseAction(json);
            }
            else if (data.Type == "throttle")
            {
                var throttleData = messageSerializer.Deserialize<JMRIMessage<ThrottleData>>(json);
                if (throttleData == null || throttleData.Data.Throttle == null)
                    throw new NotImplementedException();

                if (this.responseDict.TryRemove(throttleData.Data.Name, out responseAction))
                {
                    responseAction(json);
                }
            }

        }

        //private void HandleMessage(JToken jdata)
        //{
        //    switch (jdata.Type)
        //    {
        //        case JTokenType.Object:
        //            HandleMessage((JObject)jdata);
        //            break;

        //        case JTokenType.Array:
        //            HandleMessage((JArray)jdata);
        //            break;

        //        default:
        //            throw new NotImplementedException();

        //    }
        //}

        //private void HandleMessage(JArray jdata)
        //{
        //    foreach (var item in jdata)
        //    {
        //        HandleMessage(item);
        //    }
        //}

        //private void HandleMessage(JObject jdata)
        //{
        //    var message = se
        //    var idProperty = jdata.Properties().FirstOrDefault(x => string.Equals(x.Name, "id", StringComparison.OrdinalIgnoreCase);
        //    if(idProperty != null)
        //    {
        //        if(responseDict.TryGetValue(idProperty.Value.ToString(), out var resultAction))
        //        {

        //        }
        //    }

        //}
    }
}
