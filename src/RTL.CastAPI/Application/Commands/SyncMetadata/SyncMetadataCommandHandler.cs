using MediatR;
using Microsoft.Extensions.Logging;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.CastAPI.Application.Commands.SyncMetadata
{
    public class SyncMetadataCommandHandler : IRequestHandler<SyncMetadataCommand, SyncMetadataCommandResult>
    {
        private readonly IScrapingService _service;
        private readonly IShowsRepository _showsRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<SyncMetadataCommandHandler> _logger;


        public SyncMetadataCommandHandler(
            IScrapingService service,
            IShowsRepository showsRepository,
            IPeopleRepository peopleRepository,
            ILogger<SyncMetadataCommandHandler> logger)
        {
            _logger = logger;
            _service = service;
            _showsRepository = showsRepository;
            _peopleRepository = peopleRepository;
        }

        public async Task<SyncMetadataCommandResult> Handle(SyncMetadataCommand request, CancellationToken cancellationToken)
        {
            var isSyncCompleted = false;

            while (!isSyncCompleted)
            {
                var lastSyncId = await _showsRepository.GetMaxExternalIdAsync();

                _logger.LogInformation($"Last synced show id: {lastSyncId}");

                var shows = await _service.ScrapeBatchAsync(lastSyncId, request.SyncBatchSize);

                foreach (var show in shows)
                {
                    var model = await _showsRepository.FindByExternalIdAsync(show.Id);

                    // Apparently this show has been synced already - let's skip it
                    if (model != null) continue;

                    model = await MapToDomainModel(show);

                    await _showsRepository.AddAsync(model);
                }

                isSyncCompleted = !shows.Any();
            }

            return SyncMetadataCommandResult.Success();
        }

        private async Task<Show> MapToDomainModel(Infrastructure.TvMaze.Contract.Show show)
        {
            var model = new Show(show.Name, show.Id);

            foreach(var dto in show.Cast.Select(c => c.Person))
            {
                // use external ids to match to already synced people
                var person = await _peopleRepository.FindByExternalIdAsync(dto.Id)
                        ?? new Person(dto.Name, dto.Id, dto.Birthday);

                model.AddCastMember(person);
            }

            return model;
        }

    }
}
