using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class VehicleServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IVehicleRepository> _repositoryMock = new();
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly VehicleService _service;

    public VehicleServiceTests()
    {
        _service = new VehicleService(_repositoryMock.Object, _personRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedVehicle()
    {
        // Arrange
        var request = _fixture.Build<CreateNewVehicleRequest>()
            .With(x => x.LicensePlate, "NFA-0373")
            .Create();
        var entity = _fixture.Create<Vehicle>();
        var person = _fixture.Create<Person>();
        var createdEntity = _fixture.Create<Vehicle>();
        var dto = _fixture.Create<VehicleDto>();

        _mapperMock.Setup(m => m.Map<Vehicle>(request)).Returns(entity);
        _personRepositoryMock.Setup(x => x.GetByIdAsync(request.PersonId, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(createdEntity);
        _mapperMock.Setup(m => m.Map<VehicleDto>(createdEntity)).Returns(dto);

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Vehicle>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

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
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedVehicles()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var paginate = _fixture.Create<Paginate<Vehicle>>();
        var paginateDto = _fixture.Create<Paginate<VehicleDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<VehicleDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _service.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnVehicle_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Vehicle>();
        var dto = _fixture.Create<VehicleDto>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<VehicleDto>(entity)).Returns(dto);

        // Act
        var result = await _service.GetOneAsync(id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?) null);

        // Act
        var result = await _service.GetOneAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedVehicle_WhenFound()
    {
        // Arrange
        var input = _fixture.Create<UpdateOneVehicleInput>();
        var entity = _fixture.Create<Vehicle>();
        var updatedEntity = _fixture.Create<Vehicle>();
        var dto = _fixture.Create<VehicleDto>();

        _repositoryMock.Setup(r => r.GetByIdAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<VehicleDto>(updatedEntity)).Returns(dto);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var input = _fixture.Create<UpdateOneVehicleInput>();
        _repositoryMock.Setup(r => r.GetByIdAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?) null);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
