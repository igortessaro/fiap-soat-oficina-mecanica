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
    private readonly AvailableServiceService _service;
    private readonly Mock<ISupplyRepository> _supplyRepositoryMock = new();

    public AvailableServiceServiceTests()
    {
        _service = new AvailableServiceService(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginated()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var paginate = _fixture.Create<Paginate<AvailableService>>();
        var paginateDto = _fixture.Create<Paginate<AvailableServiceDto>>();
        string[] includes = [$"{nameof(AvailableService.AvailableServiceSupplies)}.{nameof(AvailableServiceSupply.Supply)}"];

        _repositoryMock.Setup(r => r.GetAllAsync(includes, paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
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
}
