using DiscordTrain.JMRIConnector.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Text;

namespace DiscordTrain.JMRIConnector.WebApiServices
{
    public class JMRIWebApiClient : IJMRIApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<JMRIWebApiClient> logger;

        private readonly string baseAddress;

        public JMRIWebApiClient(
            HttpClient httpClient,
            IMessageSerializer messageSerializer,
            IOptions<JMRIOptions> options,
            ILogger<JMRIWebApiClient> logger)
        {
            this.httpClient = httpClient;
            this.messageSerializer = messageSerializer;
            this.logger = logger;

            var url = options.Value.JMRIWebServerUrl;
            if (url.EndsWith("/"))            
                this.baseAddress = url + "json/";            
            else
                this.baseAddress = url + "/json/";

        }

        public async ValueTask<TOut?> GetAsync<TOut>(string address, CancellationToken cancellationToken)
        {
            var callUri = GetUri(address);
            using var response = await httpClient.GetAsync(callUri, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var data = messageSerializer.Deserialize<TOut>(json);
            return data;
        }

        public async ValueTask<TOut?> PostAsync<TIn, TOut>(string address, TIn content, CancellationToken cancellationToken)
        {
            var outJson = messageSerializer.Serialize(content);
            using var bodyContent = new StringContent(outJson, Encoding.UTF8, "application/json");

            var callUri = GetUri(address);
            using var response = await httpClient.PostAsync(callUri, bodyContent, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var data = messageSerializer.Deserialize<TOut>(json);
            return data;
        }

        private string GetUri(string address)
        {
            if(address.StartsWith("/"))
                address = address.Substring(1);

            return this.baseAddress + address;
        }
    }
}
