using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Vehicles;

public sealed class DeleteVehicleHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly DeleteVehicleHandler _useCase;

    public DeleteVehicleHandlerTests()
    {
        _useCase = new DeleteVehicleHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Vehicle>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new DeleteVehicleCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?) null);

        // Act
        var result = await _useCase.Handle(new DeleteVehicleCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
