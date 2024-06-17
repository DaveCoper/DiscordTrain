namespace DiscordTrain.Common
{
    public interface IRosterProvider
    {
        Task<IEnumerable<IRosterEntry>> GetRosterEntriesAsync(CancellationToken cancellationToken);
    }
}