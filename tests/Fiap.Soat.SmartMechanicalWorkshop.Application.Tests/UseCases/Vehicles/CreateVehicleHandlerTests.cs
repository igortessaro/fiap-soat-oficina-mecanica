using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Vehicles;

public sealed class CreateVehicleHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly CreateVehicleHandler _useCase;

    public CreateVehicleHandlerTests()
    {
        _useCase = new CreateVehicleHandler(_mapperMock.Object, _repositoryMock.Object, _personRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedVehicle()
    {
        // Arrange
        var request = _fixture.Build<CreateVehicleCommand>()
            .With(x => x.LicensePlate, VehicleFactory.CreateValidLicensePlate())
            .Create();
        var entity = new Vehicle(request.Model, request.Brand, request.ManufactureYear, request.LicensePlate, request.PersonId);
        var person = _fixture.Create<Person>();
        var dto = _fixture.Create<VehicleDto>();

        _mapperMock.Setup(m => m.Map<Vehicle>(request)).Returns(entity);
        _personRepositoryMock.Setup(x => x.GetByIdAsync(request.PersonId, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<VehicleDto>(entity)).Returns(dto);
        _repositoryMock.Setup(x => x.AddAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenPersonNotFound()
    {
        // Arrange
        var request = _fixture.Create<CreateVehicleCommand>();
        _personRepositoryMock.Setup(x => x.GetByIdAsync(request.PersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person?) null);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Should().Contain(e => e.Message.Contains("Person not found"));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenPersonIsNotClient()
    {
        // Arrange
        var request = _fixture.Create<CreateVehicleCommand>();
        var person = PeopleFactory.CreateDetailerEmployee();
        _personRepositoryMock.Setup(x => x.GetByIdAsync(request.PersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Should().Contain(e => e.Message.Contains("Only clients are allowed"));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenLicensePlateIsInvalid()
    {
        // Arrange
        var request = _fixture.Create<CreateVehicleCommand>();
        var person = PeopleFactory.CreateClient();
        var vehicle = VehicleFactory.CreateVehicle(false);
        _personRepositoryMock.Setup(x => x.GetByIdAsync(request.PersonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<Vehicle>(request)).Returns(vehicle);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Should().Contain(e => e.Message.Contains("Invalid license plate format"));
    }
}
