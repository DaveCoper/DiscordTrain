using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using DiscordTrain.Common.Model;

namespace DiscordTrain.Services;

public interface ITrainRosterProvider
{
    Task<ImmutableList<ITrain>> GetTrainRosterAsync(CancellationToken none);
}