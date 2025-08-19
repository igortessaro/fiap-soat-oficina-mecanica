using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class QuoteServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IQuoteRepository> _quoteRepositoryMock = new();
    private readonly QuoteService _service;

    public QuoteServiceTests()
    {
        _service = new QuoteService(_mapperMock.Object, _quoteRepositoryMock.Object);
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnNotFound_WhenQuoteDoesNotExist()
    {
        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Quote?) null);

        var result = await _service.PatchAsync(Guid.NewGuid(), QuoteStatus.Approved, default);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Reasons.Select(x => x.Message).Should().Contain("Quote not found");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenStatusIsAlreadySet()
    {
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Approved);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        var result = await _service.PatchAsync(Guid.NewGuid(), QuoteStatus.Approved, default);

        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Quote is already Approved");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenQuoteIsNotPending()
    {
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Rejected);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        var result = await _service.PatchAsync(Guid.NewGuid(), QuoteStatus.Approved, default);

        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Quote is not in Pending status");
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnFail_WhenStatusIsInvalid()
    {
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);

        var result = await _service.PatchAsync(Guid.NewGuid(), (QuoteStatus) 999, default);

        result.IsSuccess.Should().BeFalse();
        result.Reasons.Select(x => x.Message).Should().Contain("Invalid status 999");
    }

    [Fact]
    public async Task PatchAsync_ShouldApprove_WhenStatusIsApproved()
    {
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _quoteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Quote>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _mapperMock.Setup(m => m.Map<QuoteDto>(It.IsAny<Quote>()))
            .Returns(_fixture.Create<QuoteDto>());

        var result = await _service.PatchAsync(Guid.NewGuid(), QuoteStatus.Approved, default);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task PatchAsync_ShouldReject_WhenStatusIsRejected()
    {
        var quote = new Quote(Guid.NewGuid());
        typeof(Quote).GetProperty(nameof(Quote.Status))!.SetValue(quote, QuoteStatus.Pending);

        _quoteRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _quoteRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Quote>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(quote);
        _mapperMock.Setup(m => m.Map<QuoteDto>(It.IsAny<Quote>()))
            .Returns(_fixture.Create<QuoteDto>());

        var result = await _service.PatchAsync(Guid.NewGuid(), QuoteStatus.Rejected, default);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }
}
