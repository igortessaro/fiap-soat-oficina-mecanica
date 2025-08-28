using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Vehicles;

public sealed class ListVehiclesHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly ListVehiclesHandler _useCase;

    public ListVehiclesHandlerTests()
    {
        _useCase = new ListVehiclesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedVehicles()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListVehiclesQuery>();
        var paginate = _fixture.Create<Paginate<Vehicle>>();

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<PaginatedRequest>(), It.IsAny<CancellationToken>(), null)).ReturnsAsync(paginate);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginate);
    }
}
