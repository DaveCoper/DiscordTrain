using DiscordTrain.JMRIConnector.Messages;
using DiscordTrain.JMRIConnector.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace DiscordTrain.JMRIConnector.WebSocketServices
{
    public class JMRIWebSocketClient : IJMRIWebSocketClient
    {
        private readonly JMRIOptions options;
        private readonly ClientWebSocket webSocket;

        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<JMRIWebSocketClient> logger;

        private readonly ReaderWriterLockSlim readerWriterLock;

        private readonly ConcurrentDictionary<int, Action<string>> responseDict;
        private readonly List<WeakReference<Action<object>>> liseners;
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

            this.liseners = new List<WeakReference<Action<object>>>();

            this.webSocket = new ClientWebSocket();
            this.responseDict = new ConcurrentDictionary<int, Action<string>>();
            this.readerWriterLock = new ReaderWriterLockSlim();
        }


        public async ValueTask InitializeAsync(CancellationToken cancellationToken)
        {
            string address = GetWebsocketAddress();
            var uri = new Uri(address);
            await webSocket.ConnectAsync(
                uri,
                cancellationToken);
        }

        public async ValueTask SendAsync(JMRIMessage message, CancellationToken cancellationToken)
        {
            var json = messageSerializer.Serialize(message);
            Debug.WriteLine($"Sending: {json}");
            var bytes = Encoding.UTF8.GetBytes(json);
            await this.webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(JMRIMessage message, CancellationToken cancellationToken)
        {
            var messageNumber = Interlocked.Increment(ref this.messageCounter);
            message.Id = messageNumber;

            TaskCompletionSource<TResponse> taskCompletionSource = new TaskCompletionSource<TResponse>();

            var timeout = new CancellationTokenSource(timeoutMs);
            timeout.Token.Register(() => taskCompletionSource.TrySetCanceled(), useSynchronizationContext: false);
            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(), useSynchronizationContext: false);

            this.responseDict.TryAdd(messageNumber, (response) =>
            {
                var data = messageSerializer.Deserialize<TResponse>(response);
                taskCompletionSource.TrySetResult(data ?? throw new NotImplementedException());
            });

            await SendAsync(message, cancellationToken);
            return await taskCompletionSource.Task;
        }

        private string GetWebsocketAddress()
        {
            string address = options.WebServerUrl;
            string end = address.EndsWith("/") ? "json" : "/json";

            if (address.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                address = $"ws{options.WebServerUrl.Substring(5)}{end}";
            }
            else if (address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                address = $"ws{options.WebServerUrl.Substring(4)}{end}";
            }

            return address;
        }

        public async ValueTask ProcessMessagesAsync(byte[] buffer, CancellationToken cancellationToken)
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
            var data = messageSerializer.Deserialize(json);
            if (data == null)
                return;

            if (data is JMRIMessage message && 
                message.Id.HasValue && 
                this.responseDict.TryRemove(message.Id.Value, out var responseAction))
            {
                Debug.WriteLine($"Received response for action {message.Id}");
                responseAction(json);
                return;
            }

            this.readerWriterLock.EnterReadLock();
            try
            {
                foreach(var lisener in this.liseners)
                {
                    if(lisener.TryGetTarget(out var target))
                    {
                        target(data);
                    }
                }
            }
            finally
            {
                this.readerWriterLock.ExitReadLock();
            }
        }

        public void AddWeakMessageLisener(Action<object> lisener)
        {
            this.readerWriterLock.EnterWriteLock();
            var emptyHandle = this.liseners.FirstOrDefault(x => !x.TryGetTarget(out _));
            if (emptyHandle == null)
            {
                this.liseners.Add(new WeakReference<Action<object>>(lisener));
            }
            else
            {
                emptyHandle.SetTarget(lisener);
            }

            this.readerWriterLock.ExitWriteLock();
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
