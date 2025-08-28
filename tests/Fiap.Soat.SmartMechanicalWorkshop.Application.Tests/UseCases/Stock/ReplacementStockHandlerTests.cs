using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Stock;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Stock;

public sealed class ReplacementStockHandlerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly ReplacementStockHandler _handler;

    public ReplacementStockHandlerTests()
    {
        _handler = new ReplacementStockHandler(_mediatorMock.Object, _quoteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenStatusIsNotApproved()
    {
        // Arrange
        var quote = _fixture.Create<Quote>();
        var notification = new UpdateQuoteStatusNotification(quote.Id, quote);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _quoteRepositoryMock.Verify(r => r.GetDetailedByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateStockCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenQuoteIsNull()
    {
        // Arrange
        var quote = _fixture.Create<Quote>().Approve();
        var notification = new UpdateQuoteStatusNotification(Guid.NewGuid(), quote);
        _quoteRepositoryMock.Setup(r => r.GetDetailedByIdAsync(notification.Quote.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Quote?) null);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateStockCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenQuoteHasNoSupplies()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid()).Approve();
        var notification = new UpdateQuoteStatusNotification(quote.Id, quote);

        _quoteRepositoryMock.Setup(r => r.GetDetailedByIdAsync(quote.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateStockCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateStock_ForEachSupply_WhenApproved()
    {
        // Arrange
        var supply = new QuoteSupply(Guid.NewGuid(), Guid.NewGuid(), 100, 1);
        var quote = new Quote(Guid.NewGuid()).Approve().AddSupply(supply.SupplyId, supply.Price, supply.Quantity);
        var notification = new UpdateQuoteStatusNotification(quote.Id, quote);

        _quoteRepositoryMock.Setup(r => r.GetDetailedByIdAsync(quote.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateStockCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResponseFactory.Ok(_fixture.Build<Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies.SupplyDto>()
                .With(d => d.Quantity, 10)
                .Create(), HttpStatusCode.OK));

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.Is<UpdateStockCommand>(c =>
            c.Id == supply.SupplyId &&
            c.Quantity == supply.Quantity &&
            !c.Adding), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReplenishStock_WhenQuantityIsZeroOrNegative()
    {
        // Arrange
        var quoteSupply = new QuoteSupply(Guid.NewGuid(), Guid.NewGuid(), 100, 0);
        var quote = _fixture.Create<Quote>().Approve().AddSupply(quoteSupply.SupplyId, quoteSupply.Price, quoteSupply.Quantity);
        var notification = new UpdateQuoteStatusNotification(quote.Id, quote);

        _quoteRepositoryMock.Setup(r => r.GetDetailedByIdAsync(quote.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        _mediatorMock.SetupSequence(m => m.Send(It.IsAny<UpdateStockCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResponseFactory.Ok(_fixture.Build<Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies.SupplyDto>()
                .With(d => d.Quantity, 0)
                .Create(), HttpStatusCode.OK))
            .ReturnsAsync(ResponseFactory.Ok(_fixture.Build<Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies.SupplyDto>()
                .With(d => d.Quantity, 100)
                .Create(), HttpStatusCode.OK));

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.Is<UpdateStockCommand>(c =>
            c.Id == quoteSupply.SupplyId &&
            c.Quantity == quoteSupply.Quantity &&
            !c.Adding), It.IsAny<CancellationToken>()), Times.Once);

        _mediatorMock.Verify(m => m.Send(It.Is<UpdateStockCommand>(c =>
            c.Id == quoteSupply.SupplyId &&
            c.Quantity == 100 &&
            c.Adding), It.IsAny<CancellationToken>()), Times.Once);
    }
}
