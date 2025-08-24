using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class GetPersonByIdHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly GetPersonByIdHandler _useCase;

    public GetPersonByIdHandlerTests()
    {
        _useCase = new GetPersonByIdHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnPerson_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var person = _fixture.Create<Person>();
        var personDto = _fixture.Create<PersonDto>();
        _repositoryMock.Setup(r => r.GetDetailedByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonDto>(person)).Returns(personDto);

        // Act
        var result = await _useCase.Handle(new GetPersonByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(personDto);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Person?) null);

        // Act
        var result = await _useCase.Handle(new GetPersonByIdQuery(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
