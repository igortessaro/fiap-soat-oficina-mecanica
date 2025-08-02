using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class AvailableServiceServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
    private readonly Mock<ISupplyRepository> _supplyRepositoryMock = new();
    private readonly AvailableServiceService _service;

    public AvailableServiceServiceTests()
    {
        _service = new AvailableServiceService(_mapperMock.Object, _repositoryMock.Object, _supplyRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenNoSupplies()
    {
        // Arrange
        var request = _fixture.Build<CreateAvailableServiceRequest>()
            .With(x => x.SuppliesIds, [])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(entity)).Returns(dto);

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenSupplyDoesNotExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var request = _fixture.Build<CreateAvailableServiceRequest>()
            .With(x => x.SuppliesIds, [supplyId])
            .Create();
        var entity = _fixture.Create<AvailableService>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenAllSuppliesExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var request = _fixture.Build<CreateAvailableServiceRequest>()
            .With(x => x.SuppliesIds, [supplyId])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var supply = _fixture.Create<Supply>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync(supply);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(entity)).Returns(dto);

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
        var entity = _fixture.Create<AvailableService>();
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
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginated()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var paginate = _fixture.Create<Paginate<AvailableService>>();
        var paginateDto = _fixture.Create<Paginate<AvailableServiceDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<AvailableServiceDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _service.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<AvailableService>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _repositoryMock.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(entity)).Returns(dto);

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
        _repositoryMock.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _service.GetOneAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var input = _fixture.Create<UpdateOneAvailableServiceInput>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenSupplyNotFound()
    {
        // Arrange
        var input = _fixture.Build<UpdateOneAvailableServiceInput>()
            .With(x => x.SuppliesIds, [Guid.NewGuid()])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdated_WhenAllSuppliesExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var input = _fixture.Build<UpdateOneAvailableServiceInput>()
            .With(x => x.SuppliesIds, [supplyId])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var supply = _fixture.Create<Supply>();
        var updatedEntity = _fixture.Create<AvailableService>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync(supply);
        _repositoryMock.Setup(r => r.UpdateAsync(input.Id, input.Name, input.Price, It.IsAny<IReadOnlyList<Supply>>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(updatedEntity)).Returns(dto);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }
}
