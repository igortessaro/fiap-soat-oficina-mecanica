using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Authentication;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.Controllers;

public sealed class AuthenticationControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly IAuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _controller = new AuthenticationController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Login_ShouldSendLoginCommand_WithCorrectCredentials()
    {
        // Arrange
        var person = PeopleFactory.CreateClient();
        var request = new LoginRequest(person.Email, person.Password);
        _mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response<string>("success", HttpStatusCode.OK));

        // Act
        var result = await _controller.Login(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectResult>();
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<LoginCommand>(c => c.Email == request.Email && c.Password == request.Password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
