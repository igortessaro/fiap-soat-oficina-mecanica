using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class QuoteServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ILogger<QuoteService>> _loggerMock = new();
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock = new();
    private readonly QuoteService _service;

    public QuoteServiceTests()
    {
        _service = new QuoteService(_loggerMock.Object, _quoteRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenStatusIsNotWaitingApproval()
    {
        // Arrange
        var serviceOrder = _fixture.Build<ServiceOrderDto>()
            .With(x => x.Status, EServiceOrderStatus.Delivered)
            .Create();

        // Act
        var result = await _service.CreateAsync(serviceOrder, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenNoAvailableServices()
    {
        // Arrange
        var serviceOrder = _fixture.Build<ServiceOrderDto>()
            .With(x => x.Status, EServiceOrderStatus.WaitingApproval)
            .With(x => x.AvailableServices, Array.Empty<AvailableServiceDto>())
            .Create();

        // Act
        var result = await _service.CreateAsync(serviceOrder, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var availableService = _fixture.Build<AvailableServiceDto>()
            .With(x => x.Supplies, _fixture.CreateMany<SupplyDto>(2).ToArray())
            .Create();
        var serviceOrder = _fixture.Build<ServiceOrderDto>()
            .With(x => x.Status, EServiceOrderStatus.WaitingApproval)
            .With(x => x.AvailableServices, [availableService])
            .Create();

        var expectedQuote = new Quote(serviceOrder.Id);
        foreach (var svc in serviceOrder.AvailableServices)
            expectedQuote.AddService(svc.Id, svc.Price);
        foreach (var supply in serviceOrder.AvailableServices.SelectMany(x => x.Supplies))
            expectedQuote.AddSupply(supply.Id, supply.Price, supply.Quantity);

        _quoteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Quote>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedQuote);

        // Act
        var result = await _service.CreateAsync(serviceOrder, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }
}
