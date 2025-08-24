using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Quotes;

public sealed class UpdateQuoteStatusHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock = new();
    private readonly UpdateQuoteStatusHandler _useCase;

    public UpdateQuoteStatusHandlerTests()
    {
        _useCase = new UpdateQuoteStatusHandler(_mapperMock.Object, _mediatorMock.Object, _quoteRepositoryMock.Object);
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnNotFound_WhenQuoteDoesNotExist()
    {
        // Arrange
        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Quote?) null);

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), QuoteStatus.Approved, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Reasons.Select(x => x.Message).Should().Contain("Quote not found");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenStatusIsAlreadySet()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Approved);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), QuoteStatus.Approved, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Quote is already Approved");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenQuoteIsNotPending()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Rejected);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), QuoteStatus.Approved, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Quote is not in Pending status");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenStatusIsInvalid()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), (QuoteStatus) 999, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Invalid status 999");
    }

    [Fact]
    public async Task PatchAsync_ShouldApprove_WhenStatusIsApproved()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _quoteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Quote>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _mapperMock.Setup(m => m.Map<QuoteDto>(It.IsAny<Quote>()))
            .Returns(_fixture.Create<QuoteDto>());

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), QuoteStatus.Approved, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task PatchAsync_ShouldReject_WhenStatusIsRejected()
    {
        // Arrange
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _quoteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Quote>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _mapperMock.Setup(m => m.Map<QuoteDto>(It.IsAny<Quote>()))
            .Returns(_fixture.Create<QuoteDto>());

        // Act
        var result = await _useCase.Handle(new UpdateQuoteStatusCommand(Guid.NewGuid(), QuoteStatus.Rejected, Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }
}
