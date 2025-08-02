using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class AuthServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<IPersonService> _personServiceMock = new();
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_test_key_1234567890");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _service = new AuthService(_configMock.Object, _personServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = _fixture.Create<LoginRequest>();
        var personDto = _fixture.Build<PersonDto>()
            .With(x => x.EmployeeRole, EmployeeRole.Detailer)
            .Create();
        var response = ResponseFactory.Ok(personDto);

        _personServiceMock.Setup(s => s.GetOneByLoginAsync(loginRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _service.Login(loginRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginRequest = _fixture.Create<LoginRequest>();
        var response = ResponseFactory.Fail<PersonDto>(new FluentResults.Error("Person Not Found"), HttpStatusCode.NotFound);

        _personServiceMock.Setup(s => s.GetOneByLoginAsync(loginRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _service.Login(loginRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Data.Should().BeNull();
    }
}
