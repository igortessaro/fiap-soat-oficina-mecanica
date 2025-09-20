using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class CreatePersonHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly CreatePersonHandler _useCase;

    public CreatePersonHandlerTests()
    {
        _useCase = new CreatePersonHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedPerson()
    {
        // Arrange
        var request = _fixture.Build<CreatePersonCommand>()
            .With(x => x.PersonType, PersonType.Client)
            .Without(x => x.EmployeeRole)
            .Create();
        var person = PeopleFactory.CreateClient();

        _mapperMock.Setup(m => m.Map<Person>(request)).Returns(person);
        _repositoryMock.Setup(r => r.AddAsync(person, It.IsAny<CancellationToken>())).ReturnsAsync(person);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(person);
    }
}
