using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class AuthServiceTests
{
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_test_key_1234567890");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _service = new AuthService(_configMock.Object, _personRepositoryMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var person = PeopleFactory.CreateDetailerEmployee();
        var loginRequest = _fixture.Build<LoginRequest>()
            .With(x => x.Email, (string)person.Email)
            .With(x => x.Password, PeopleFactory.ValidPassword)
            .Create();

        _personRepositoryMock.Setup(s => s.GetByEmailAsync(loginRequest.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _service.LoginAsync(loginRequest, CancellationToken.None);

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
        var loginRequest = _fixture.Build<LoginRequest>()
            .With(x => x.Email, (string)person.Email)
            .With(x => x.Password, "123456789")
            .Create();

        _personRepositoryMock.Setup(s => s.GetByEmailAsync(loginRequest.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        // Act
        var result = await _service.LoginAsync(loginRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Data.Should().BeNull();
    }
}
