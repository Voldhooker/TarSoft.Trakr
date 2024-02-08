using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using TarSoft.GpsUnit.Api.CQRS.Queries;
using TarSoft.GpsUnit.Infrastructure;
using TarSoft.Trakr.Common;

namespace TarSoft.Tests
{
    public class GetGpsUnitQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingUnit_ReturnsOkResult()
        {
            // Arrange
            var query = new GetGpsUnitQuery { Id = Guid.NewGuid() };
            var cancellationToken = CancellationToken.None;

            var dbContextMock = new Mock<GpsUnitContext>();
            var gpsUnitMock = new Mock<DbSet<GpsUnit.Domain.GpsUnit>>();

            var existingUnit = new GpsUnit.Domain.GpsUnit(Guid.NewGuid(), Guid.NewGuid(), "", "");
            gpsUnitMock.Setup(m => m.FindAsync(query.Id, cancellationToken)).ReturnsAsync(existingUnit);

            dbContextMock.Setup(m => m.GpsUnits).Returns(gpsUnitMock.Object);

            var handler = new GetGpsUnitHandler(dbContextMock.Object);

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(existingUnit, result.Value);
        }

        [Fact]
        public async Task Handle_NonExistingUnit_ReturnsFailResult()
        {
            // Arrange
            var query = new GetGpsUnitQuery { Id = Guid.NewGuid() };
            var cancellationToken = CancellationToken.None;

            var dbContextMock = new Mock<GpsUnitContext>();
            var gpsUnitMock = new Mock<DbSet<GpsUnit.Domain.GpsUnit>>();

            gpsUnitMock.Setup(m => m.FindAsync(query.Id, cancellationToken)).ReturnsAsync((GpsUnit.Domain.GpsUnit?)null);

            dbContextMock.Setup(m => m.GpsUnits).Returns(gpsUnitMock.Object);

            var handler = new GetGpsUnitHandler(dbContextMock.Object);

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Unit not found", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsFailResult()
        {
            // Arrange
            var query = new GetGpsUnitQuery { Id = Guid.NewGuid() };
            var cancellationToken = CancellationToken.None;

            var dbContextMock = new Mock<GpsUnitContext>();
            var gpsUnitMock = new Mock<DbSet<GpsUnit.Domain.GpsUnit>>();

            gpsUnitMock.Setup(m => m.FindAsync(query.Id, cancellationToken)).ThrowsAsync(new Exception());

            dbContextMock.Setup(m => m.GpsUnits).Returns(gpsUnitMock.Object);

            var handler = new GetGpsUnitHandler(dbContextMock.Object);

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            Assert.True(result.IsFailed);
            Assert.IsType<DatabaseError>(result.Errors[0]);
        }
    }
}