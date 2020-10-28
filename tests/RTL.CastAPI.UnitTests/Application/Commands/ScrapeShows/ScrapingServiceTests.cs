using Moq;
using RTL.CastAPI.Application.Commands.SyncMetadata;
using RTL.CastAPI.Infrastructure.TvMaze;
using RTL.CastAPI.Infrastructure.TvMaze.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.CastAPI.UnitTests
{
    public class ScrapingServiceTests
    {
        private ScrapingService _target;

        private readonly Mock<ITvMazeHttpClient> _httpClient;

        public ScrapingServiceTests()
        {
            _httpClient = new Mock<ITvMazeHttpClient>();

            _target = new ScrapingService(_httpClient.Object);
        }

        [Fact]
        public async Task ScrapeBatchAsync_WhenBatchResultShowIdsAreLessThanGivenShowId_ReturnsEmptyCollection()
        {
            // Arrange
            int showId = 20;
            int batchSize = 250;

            _httpClient
                .Setup(c => c.GetShowIndexPage(0))
                .ReturnsAsync(GenerateMockShows(1, 20));

            // Act
            var results = await _target.ScrapeBatchAsync(showId, batchSize);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task ScrapeBatchAsync_WhenSomeBatchResultShowIdsAreLessThanGivenShowId_ReturnsOnlyShowWithIdGreaterThanGivenId()
        {
            // Arrange
            int showId = 20;
            int batchSize = 250;


            _httpClient
                .Setup(c => c.GetShowIndexPage(0))
                .ReturnsAsync(GenerateMockShows(1, 23));

            // Act
            var results = await _target.ScrapeBatchAsync(showId, batchSize);

            // Assert
            Assert.NotEmpty(results);
            Assert.Equal(3, results.Count());
            Assert.True(results.All(s => s.Id > showId));
        }

        private List<Show> GenerateMockShows(int start, int end)
            => Enumerable.Range(start, end).Select(id => Mock.Of<Show>(m => m.Id == id)).ToList();


    }
}
