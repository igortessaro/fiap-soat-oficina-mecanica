using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.AvailableServices;

public sealed class UpdateAvailableServiceHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
    private readonly Mock<ISupplyRepository> _supplyRepositoryMock = new();
    private readonly UpdateAvailableServiceHandler _useCase;

    public UpdateAvailableServiceHandlerTests()
    {
        _useCase = new UpdateAvailableServiceHandler(_mapperMock.Object, _repositoryMock.Object, _supplyRepositoryMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var command = _fixture.Create<UpdateAvailableServiceCommand>();
        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenSupplyNotFound()
    {
        // Arrange
        var command = _fixture.Build<UpdateAvailableServiceCommand>()
            .With(x => x.Supplies, [new UpdateServiceSupplyCommand(Guid.NewGuid(), 10)])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdated_WhenAllSuppliesExist()
    {
        // Arrange
        var supplyId = Guid.NewGuid();
        var command = _fixture.Build<UpdateAvailableServiceCommand>()
            .With(x => x.Supplies, [new UpdateServiceSupplyCommand(supplyId, 10)])
            .Create();
        var entity = _fixture.Create<AvailableService>();
        var supply = _fixture.Create<Supply>();
        var updatedEntity = _fixture.Create<AvailableService>();
        var dto = _fixture.Create<AvailableServiceDto>();

        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _supplyRepositoryMock.Setup(s => s.GetByIdAsync(supplyId, It.IsAny<CancellationToken>())).ReturnsAsync(supply);
        _repositoryMock.Setup(r => r.UpdateAsync(command.Id, command.Name, command.Price, It.IsAny<IReadOnlyList<ServiceSupplyDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<AvailableServiceDto>(updatedEntity)).Returns(dto);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }
}
