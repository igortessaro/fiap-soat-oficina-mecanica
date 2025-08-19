using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Vehicles;

public sealed class ListVehiclesHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly ListVehiclesHandler _useCase;

    public ListVehiclesHandlerTests()
    {
        _useCase = new ListVehiclesHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedVehicles()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListVehiclesQuery>();
        var paginate = _fixture.Create<Paginate<Vehicle>>();
        var paginateDto = _fixture.Create<Paginate<VehicleDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<VehicleDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }
}
