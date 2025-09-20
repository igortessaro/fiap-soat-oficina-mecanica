using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.ServiceOrders;

public sealed class GetServiceOrderByIdHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IServiceOrderRepository> _repositoryMock = new();
    private readonly GetServiceOrderByIdHandler _useCase;

    public GetServiceOrderByIdHandlerTests()
    {
        _useCase = new GetServiceOrderByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<ServiceOrder>();
        _repositoryMock.Setup(r => r.GetDetailedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new GetServiceOrderByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(entity);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetDetailedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        // Act
        var result = await _useCase.Handle(new GetServiceOrderByIdQuery(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
