using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Tests.Controllers;

public sealed class QuoteControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly QuoteController _controller;

    public QuoteControllerTests()
    {
        _controller = new QuoteController(_mediatorMock.Object);
    }

    [Fact]
    public async Task PatchQuoteAsync_ShouldReturnOk_WhenQuoteStatusIsUpdated()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var quoteId = _fixture.Create<Guid>();
        var status = _fixture.Create<QuoteStatus>();
        var response = ResponseFactory.Ok<QuoteDto>(_fixture.Create<QuoteDto>());

        _mediatorMock.Setup(m => m.Send(
                It.Is<UpdateQuoteStatusCommand>(c => c.Id == quoteId && c.Status == status && c.ServiceOrderId == id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.PatchQuoteAsync(id, quoteId, status, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }
}
