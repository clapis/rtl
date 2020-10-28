using Microsoft.Extensions.Logging;
using Moq;
using RTL.CastAPI.Application.Commands.ScrapeShows;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Infrastructure.TvMaze;
using RTL.CastAPI.Model;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.CastAPI.UnitTests
{
    public class ScrapeShowsCommandHandlerTests
    {
        private ScrapeShowsCommandHandler _target;

        private readonly Mock<ITvMazeHttpClient> _httpClient;
        private readonly Mock<ICastRepository> _repository;
        private readonly Mock<ILogger<ScrapeShowsCommandHandler>> _logger;

        public ScrapeShowsCommandHandlerTests()
        {
            _httpClient = new Mock<ITvMazeHttpClient>();
            _repository = new Mock<ICastRepository>();
            _logger = new Mock<ILogger<ScrapeShowsCommandHandler>>();

            _target = new ScrapeShowsCommandHandler(_httpClient.Object, _repository.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_WhenLocalStorageHasNoScrapedShows_QueryExternalSourceFirstPage()
        {
            // Arrange
            int? lastSyncId = null;

            _repository
                .Setup(c => c.GetMaxExternalIdAsync())
                .ReturnsAsync(lastSyncId);

            _httpClient
                .Setup(c => c.GetShowIndexPage(It.IsAny<int>()))
                .ReturnsAsync(Enumerable.Empty<Show>());

            // Act
            await _target.Handle(new ScrapeShowsCommand(), default);

            // Assert
            _httpClient.Verify(s => s.GetShowIndexPage(0), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurrentIndexPageIsUpToDate_CastingInformationIsNotQueried()
        {
            // Arrange
            var page = 0;
            var lastSyncId = 42;
            var pageResults = Enumerable.Range(1, lastSyncId)
                .Select(id => new Show { Id = id });

            _repository
                .Setup(c => c.GetMaxExternalIdAsync())
                .ReturnsAsync(lastSyncId);

            _httpClient
                .Setup(c => c.GetShowIndexPage(page))
                .ReturnsAsync(pageResults);

            // Act
            await _target.Handle(new ScrapeShowsCommand(), default);

            // Assert
            _httpClient.Verify(s => s.GetShowCastAsync(It.IsAny<int>()), Times.Never);
            _repository.Verify(s => s.AddAsync(It.IsAny<Show>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCurrentIndexPageReachesEnd_NextPageIsQueried()
        {
            // Arrange
            var page = 0;
            var lastSyncId = 240;
            var pageResults = Enumerable.Range(1, 250).Select(id => new Show { Id = id });

            _repository
                .Setup(c => c.GetMaxExternalIdAsync())
                .ReturnsAsync(() => lastSyncId);

            _repository
                .Setup(c => c.AddAsync(It.IsAny<Show>()))
                .Callback<Show>(s => lastSyncId = s.Id);

            _httpClient
                .Setup(c => c.GetShowCastAsync(It.IsAny<int>()))
                .ReturnsAsync(new[] { new CastMember { Person = new Person() } } );

            _httpClient
                .Setup(c => c.GetShowIndexPage(page))
                .ReturnsAsync(pageResults);

            _httpClient
                .Setup(c => c.GetShowIndexPage(page + 1))
                .ReturnsAsync(Enumerable.Empty<Show>());

            // Act
            await _target.Handle(new ScrapeShowsCommand(), default);

            // Assert
            _httpClient.Verify(s => s.GetShowIndexPage(page), Times.Once);
            _httpClient.Verify(s => s.GetShowIndexPage(page + 1), Times.Once);
        }

    }
}
