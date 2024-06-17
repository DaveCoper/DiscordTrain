namespace DiscordTrain.JMRIConnector.WebApiServices
{
    public interface IJMRIWebApiClient
    {
        ValueTask<TOut?> GetAsync<TOut>(string address, CancellationToken cancellationToken);

        ValueTask<TOut?> PostAsync<TIn, TOut>(string address, TIn content, CancellationToken cancellationToken);
    }
}