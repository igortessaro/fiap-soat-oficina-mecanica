using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class GetSupplyByIdHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly GetSupplyByIdHandler _useCase;

    public GetSupplyByIdHandlerTests()
    {
        _useCase = new GetSupplyByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnSupply_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Supply>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new GetSupplyByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(entity);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _useCase.Handle(new GetSupplyByIdQuery(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
