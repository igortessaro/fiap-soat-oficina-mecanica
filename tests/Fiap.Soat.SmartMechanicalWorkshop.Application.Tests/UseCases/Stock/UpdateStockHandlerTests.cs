using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Stock;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Stock;

public sealed class UpdateStockHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISupplyRepository> _supplyRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly UpdateStockHandler _handler;

    public UpdateStockHandlerTests()
    {
        _handler = new UpdateStockHandler(_mapperMock.Object, _supplyRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSupplyDoesNotExist()
    {
        var command = _fixture.Create<UpdateStockCommand>();
        _supplyRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supply?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldAddToStock_WhenAddingIsTrue()
    {
        var command = _fixture.Build<UpdateStockCommand>()
            .With(x => x.Adding, true)
            .Create();
        var supply = _fixture.Create<Supply>();
        var updatedSupply = _fixture.Create<Supply>();
        var dto = _fixture.Create<SupplyDto>();

        _supplyRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supply);
        _supplyRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Supply>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedSupply)
            .Callback<Supply, CancellationToken>((s, _) => s.Should().BeEquivalentTo(supply.AddToStock(command.Quantity)));
        _mapperMock.Setup(m => m.Map<SupplyDto>(supply)).Returns(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task Handle_ShouldRemoveFromStock_WhenAddingIsFalse()
    {
        var command = _fixture.Build<UpdateStockCommand>()
            .With(x => x.Adding, false)
            .Create();
        var supply = _fixture.Create<Supply>();
        var updatedSupply = _fixture.Create<Supply>();
        var dto = _fixture.Create<SupplyDto>();

        _supplyRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supply);
        _supplyRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Supply>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedSupply)
            .Callback<Supply, CancellationToken>((s, _) => s.Should().BeEquivalentTo(supply.RemoveFromStock(command.Quantity)));
        _mapperMock.Setup(m => m.Map<SupplyDto>(supply)).Returns(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }
}
