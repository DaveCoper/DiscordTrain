using DiscordTrain.Common.Commands;
using DiscordTrain.JMRIConnector.WebApiServices;

using Microsoft.AspNetCore.Mvc;

namespace BlazorTrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        // GET api/image or GET api/image/{id}
        [HttpGet]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(string id, [FromServices] IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var tasks = serviceProvider
                .GetKeyedServices<IListRosterCommand>(KeyedService.AnyKey)
                .Select(x => x.ListRosterAsync(cancellationToken));

            var results = await Task.WhenAll(tasks);
            var entries = results.SelectMany(x => x).ToList();
            var entry = entries.Find(x => x.Id == id);

            if (string.IsNullOrEmpty(entry?.SmallIcon))
                return NotFound();

            var jmriWebApi = serviceProvider.GetRequiredKeyedService<IJMRIWebApiClient>("JMRI");
            var response = await jmriWebApi.GetAsync(entry.SmallIcon, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();
            return File(stream, "image/png");
        }
    }
}
