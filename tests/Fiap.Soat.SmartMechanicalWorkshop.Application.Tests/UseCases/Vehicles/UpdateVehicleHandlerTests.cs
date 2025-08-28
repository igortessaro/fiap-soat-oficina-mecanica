using AutoFixture;
using Bogus;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Vehicles;

public sealed class UpdateVehicleHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly UpdateVehicleHandler _useCase;

    public UpdateVehicleHandlerTests()
    {
        _useCase = new UpdateVehicleHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedVehicle_WhenFound()
    {
        // Arrange
        var faker = new Faker("pt_BR");
        var entity = VehicleFactory.CreateVehicle();
        var command = new UpdateVehicleCommand(
            entity.Id,
            VehicleFactory.CreateValidLicensePlate(),
            entity.ManufactureYear - 2,
            faker.Vehicle.Manufacturer(),
            faker.Vehicle.Model(),
            entity.PersonId);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(entity);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var command = _fixture.Create<UpdateVehicleCommand>();
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?) null);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnBadRequest_WhenLicensePlateIsInvalid()
    {
        // Arrange
        var command = _fixture.Create<UpdateVehicleCommand>();
        var entity = VehicleFactory.CreateVehicle(false);
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Never);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Should().Contain(e => e.Message.Contains("Invalid license plate format"));
    }
}
