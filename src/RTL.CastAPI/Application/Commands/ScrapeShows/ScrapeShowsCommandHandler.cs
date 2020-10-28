using MediatR;
using Microsoft.Extensions.Logging;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Infrastructure.TvMaze;
using RTL.CastAPI.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.CastAPI.Application.Commands.ScrapeShows
{
    public class ScrapeShowsCommandHandler : IRequestHandler<ScrapeShowsCommand, ScrapeShowsCommandResult>
    {
        public const int IndexPageSize = 250;

        private readonly ITvMazeHttpClient _client;
        private readonly ICastRepository _repository;
        private readonly ILogger<ScrapeShowsCommandHandler> _logger;

        public ScrapeShowsCommandHandler(
            ITvMazeHttpClient client,
            ICastRepository repository,
            ILogger<ScrapeShowsCommandHandler> logger)
        {
            _client = client;
            _repository = repository;
            _logger = logger;
        }

        public async Task<ScrapeShowsCommandResult> Handle(ScrapeShowsCommand request, CancellationToken cancellationToken)
        {
            var isSyncCompleted = false;

            while (!isSyncCompleted)
                isSyncCompleted = await SynchronizeAsync();

            return ScrapeShowsCommandResult.Success();
        }


        /// <summary>
        /// Synchronizes Shows with the external source (TvMaze). It will sync up to 250 Shows. 
        /// If there are still more shows to be synced, it returns false (e.g. more than 250 new shows).
        /// Otherwise, if sync is completed, it returns true;
        /// </summary>
        private async Task<bool> SynchronizeAsync()
        {
            // Retrieve the last synced ShowId 
            var lastSyncId = await _repository.GetMaxExternalIdAsync() ?? 0;

            _logger.LogInformation($"Last synced show id: {lastSyncId}");

            // Calculate from which page the sync should continue
            var page = GetIndexPageForId(lastSyncId + 1);

            _logger.LogInformation($"Querying index page: {page}");

            // Retrieve shows in that page
            var shows = await _client.GetShowIndexPage(page);

            _logger.LogInformation($"Page {page} has {shows.Count()} shows.");

            // Exclude shows in the page that we have already synced
            var unsynced = shows.Where(show => show.Id > lastSyncId)
                                .OrderBy(show => show.Id);

            // Load Cast information for each show :/
            // TODO: It seems that we cannot request embedded cast information when
            // querying the index, but it's worth looking further to avoid this n+1
            foreach (var show in unsynced)
            {
                await LoadCastAsync(show);

                await _repository.AddAsync(show);

                // keep track of last synced id, so that we check if there are more pages to be synced
                lastSyncId = show.Id;
            }

            // If the page of the next item is still this page, sync is completed.
            return GetIndexPageForId(lastSyncId + 1) == page;
        }

        private async Task LoadCastAsync(Show show)
        {
            var cast = await _client.GetShowCastAsync(show.Id);

            _logger.LogInformation($"Retrieving cast information for ShowId {show.Id}");

            show.Cast = cast.ToList();
        }

        /// <summary>
        /// Retrieve the page on the TvMaze index where for the given Id
        /// </summary>
        private int GetIndexPageForId(int id)
        {
            return id / IndexPageSize;
        }

    }
}
