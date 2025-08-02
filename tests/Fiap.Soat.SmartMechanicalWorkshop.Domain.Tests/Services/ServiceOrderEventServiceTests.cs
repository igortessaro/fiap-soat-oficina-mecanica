using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class ServiceOrderEventServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IServiceOrderEventRepository> _repositoryMock = new();
    private readonly ServiceOrderEventService _service;

    public ServiceOrderEventServiceTests()
    {
        _service = new ServiceOrderEventService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddEventAndReturnOk()
    {
        // Arrange
        var serviceOrderId = Guid.NewGuid();
        var status = _fixture.Create<ServiceOrderStatus>();
        var eventEntity = new ServiceOrderEvent(serviceOrderId, status);

        _repositoryMock
            .Setup(r => r.AddAsync(It.Is<ServiceOrderEvent>(e => e.ServiceOrderId == serviceOrderId && e.Status == status), It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);

        // Act
        var result = await _service.CreateAsync(serviceOrderId, status, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ServiceOrderEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
