using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.AvailableServices;

public sealed class GetAvailableServiceByIdHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
    private readonly GetAvailableServiceByIdHandler _useCase;

    public GetAvailableServiceByIdHandlerTests()
    {
        _useCase = new GetAvailableServiceByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<AvailableService>();

        _repositoryMock.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new GetAvailableServiceByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(entity);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _useCase.Handle(new GetAvailableServiceByIdQuery(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
