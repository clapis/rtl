using RTL.CastAPI.Infrastructure.TvMaze;
using RTL.CastAPI.Infrastructure.TvMaze.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.CastAPI.Application.Commands.SyncMetadata
{
    public interface IScrapingService
    {
        /// <summary>
        /// Scrapes for shows with id greater than the given show id.
        /// Limits the number of shows returned by the specified batch size.
        /// </summary>
        Task<IEnumerable<Show>> ScrapeBatchAsync(int showId, int batchSize);
    }

    public class ScrapingService : IScrapingService
    {
        private const int IndexPageSize = 250;

        private readonly ITvMazeHttpClient _client;

        public ScrapingService(ITvMazeHttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<Show>> ScrapeBatchAsync(int showId, int batchSize)
        {
            var page = GetIndexPageForId(showId);

            var shows = await _client.GetShowIndexPage(page);

            // Exclude shows in the page that we have already synced
            var unsynced = shows.Where(show => show.Id > showId)
                                .OrderBy(show => show.Id)
                                .Take(batchSize)
                                .ToList();

            // It seems like we can't request embedded cast information
            // when querying the index. It's worth looking further into
            // it to avoid this n+1 extra calls to load each show's cast.
            foreach (var show in unsynced)
                show.Cast = await _client.GetShowCastAsync(show.Id);

            return unsynced;
        }

        /// <summary>
        /// Calculate the page of a given Id
        /// </summary>
        private int GetIndexPageForId(int id)
        {
            return id / IndexPageSize;
        }

    }
}
