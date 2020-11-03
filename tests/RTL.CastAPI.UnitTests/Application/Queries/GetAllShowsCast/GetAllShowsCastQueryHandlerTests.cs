using AutoMapper;
using Moq;
using RTL.CastAPI.Application.Queries.GetAllShowsCast;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.CastAPI.UnitTests.Application.Queries.GetAllShowsCast
{
    public class GetAllShowsCastQueryHandlerTests
    {
        private GetAllShowsCastQueryHandler _target;

        private readonly Mock<IShowsRepository> _repository;
        private readonly MapperConfiguration _mappingConfiguration; 

        public GetAllShowsCastQueryHandlerTests()
        {
            _repository = new Mock<IShowsRepository>();
            _mappingConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _target = new GetAllShowsCastQueryHandler(_repository.Object, _mappingConfiguration.CreateMapper());
        }

        [Fact]
        public void Mapping_Configuration_IsValid()
        {
            _mappingConfiguration.AssertConfigurationIsValid();
        }

        [Fact]
        public async Task Handle_Always_ReturnsCastOrderedByDescendingBirthday()
        {
            // Arrange
            var theWire = new Show("The Wire", 42);
            theWire.AddCastMember(new Person("Idris Elba", 3, DateTime.Parse("1970-09-22")));
            theWire.AddCastMember(new Person("Michael K. Williams", 4, DateTime.Parse("1970-09-23")));

            _repository
                .Setup(r => r.GetPageAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Show> { theWire });

            // Act
            var result = await _target.Handle(new GetAllShowsQuery(0), default);

            // Assert
            foreach(var show in result.Shows)
            {
                var expected = show.Cast.OrderByDescending(c => c.Birthday);
                Assert.True(show.Cast.SequenceEqual(expected));
            }
        }
    }
}
