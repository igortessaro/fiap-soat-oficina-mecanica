using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.EventLog;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.EventLog;

public sealed class CreateEventLogHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IServiceOrderEventRepository> _repositoryMock = new();
    private readonly CreateEventLogHandler _useCase;

    public CreateEventLogHandlerTests()
    {
        _useCase = new CreateEventLogHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddEventAndReturnOk()
    {
        // Arrange
        var serviceOrderId = Guid.NewGuid();
        var serviceOrder = _fixture.Create<ServiceOrderDto>();
        var eventEntity = new ServiceOrderEvent(serviceOrderId, serviceOrder.Status);

        _repositoryMock
            .Setup(r => r.AddAsync(It.Is<ServiceOrderEvent>(e => e.ServiceOrderId == serviceOrderId && e.Status == serviceOrder.Status), It.IsAny<CancellationToken>()))
            .ReturnsAsync(eventEntity);

        // Act
        await _useCase.Handle(new UpdateServiceOrderStatusNotification(serviceOrderId, serviceOrder), CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ServiceOrderEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
