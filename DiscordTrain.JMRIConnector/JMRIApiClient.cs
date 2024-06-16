using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTrain.JMRIConnector
{
    public class JMRIApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<JMRIApiClient> logger;

        public JMRIApiClient(HttpClient httpClient, IMessageSerializer messageSerializer, ILogger<JMRIApiClient> logger)
        {
            this.httpClient = httpClient;
            this.messageSerializer = messageSerializer;

            this.logger = logger;
        }

        public async Task<TOutput> GetAsync<TOutput>(string address, CancellationToken cancellationToken)
        {
            using var response = await httpClient.GetAsync(address, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            var data = this.messageSerializer.Deserialize<TOutput>(json);
        }
    }
}
