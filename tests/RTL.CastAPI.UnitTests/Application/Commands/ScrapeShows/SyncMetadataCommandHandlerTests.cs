using Microsoft.Extensions.Logging;
using Moq;
using RTL.CastAPI.Application.Commands.SyncMetadata;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Infrastructure.TvMaze.Contract;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.CastAPI.UnitTests
{
    public class SyncMetadataCommandHandlerTests
    {
        private SyncMetadataCommandHandler _target;

        private readonly Mock<IScrapingService> _service;
        private readonly Mock<IShowsRepository> _showsRepository;
        private readonly Mock<IPeopleRepository> _peopleRepository;
        private readonly Mock<ILogger<SyncMetadataCommandHandler>> _logger;

        public SyncMetadataCommandHandlerTests()
        {
            _service = new Mock<IScrapingService>();
            _showsRepository = new Mock<IShowsRepository>();
            _peopleRepository = new Mock<IPeopleRepository>();
            _logger = new Mock<ILogger<SyncMetadataCommandHandler>>();

            _target = new SyncMetadataCommandHandler(_service.Object, 
                _showsRepository.Object, _peopleRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_WhenScrapingProducesNoResults_TerminatesSuccessfully()
        {
            // Arrange
            int lastSyncId = 0;

            _showsRepository
                .Setup(c => c.GetMaxExternalIdAsync())
                .ReturnsAsync(lastSyncId);

            _service
                .Setup(c => c.ScrapeBatchAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(Enumerable.Empty<Show>());

            // Act
            await _target.Handle(new SyncMetadataCommand(), default);

            // Assert
            _service.Verify(s => s.ScrapeBatchAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }


    }
}
