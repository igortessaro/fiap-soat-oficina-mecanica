using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class GetSupplyByIdHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly GetSupplyByIdHandler _useCase;

    public GetSupplyByIdHandlerTests()
    {
        _useCase = new GetSupplyByIdHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnSupply_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Supply>();
        var dto = _fixture.Create<SupplyDto>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<SupplyDto>(entity)).Returns(dto);

        // Act
        var result = await _useCase.Handle(new GetSupplyByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
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
