using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.AvailableServices;

public sealed class CreateAvailableServiceHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
    private readonly Mock<ISupplyRepository> _supplyRepositoryMock = new();
    private readonly CreateAvailableServiceHandler _useCase;

    public CreateAvailableServiceHandlerTests()
    {
        _useCase = new CreateAvailableServiceHandler(_mapperMock.Object, _repositoryMock.Object, _supplyRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenNoSupplies()
    {
        // Arrange
        var request = _fixture.Build<CreateAvailableServiceCommand>()
            .With(x => x.Supplies, [])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(entity)).Returns(dto);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenSupplyDoesNotExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var request = _fixture.Build<CreateAvailableServiceCommand>()
            .With(x => x.Supplies, [new CreateServiceSupplyCommand(supplyId, 10)])
            .Create();
        var entity = _fixture.Create<AvailableService>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenAllSuppliesExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var request = _fixture.Build<CreateAvailableServiceCommand>()
            .With(x => x.Supplies, [new CreateServiceSupplyCommand(supplyId, 10)])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var supply = _fixture.Create<Supply>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _mapperMock.Setup(m => m.Map<AvailableService>(request)).Returns(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync(supply);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(entity)).Returns(dto);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }
}
