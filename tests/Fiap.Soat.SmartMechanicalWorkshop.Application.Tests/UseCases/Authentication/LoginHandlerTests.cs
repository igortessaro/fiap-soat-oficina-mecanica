using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Authentication;

public sealed class LoginHandlerTests
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly LoginHandler _useCase;

    public LoginHandlerTests()
    {
        _configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_test_key_1234567890");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _useCase = new LoginHandler(_configMock.Object, _personRepositoryMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var person = PeopleFactory.CreateDetailerEmployee();
        var command = _fixture.Build<LoginCommand>()
            .With(x => x.Email, (string) person.Email)
            .With(x => x.Password, PeopleFactory.ValidPassword)
            .Create();

        _personRepositoryMock.Setup(s => s.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var person = PeopleFactory.CreateDetailerEmployee();
        var command = _fixture.Build<LoginCommand>()
            .With(x => x.Email, (string) person.Email)
            .With(x => x.Password, "123456789")
            .Create();

        _personRepositoryMock.Setup(s => s.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Data.Should().BeNull();
    }
}
