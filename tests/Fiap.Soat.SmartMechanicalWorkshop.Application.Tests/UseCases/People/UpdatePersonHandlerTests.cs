using AutoFixture;
using AutoMapper;
using Bogus;
using Bogus.Extensions.Brazil;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Moq;
using System.Net;
using Person = Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities.Person;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class UpdatePersonHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly UpdatePersonHandler _useCase;

    public UpdatePersonHandlerTests()
    {
        _useCase = new UpdatePersonHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedPerson_WhenExists()
    {
        // Arrange
        var faker = new Faker("pt_BR");
        var input = _fixture.Build<UpdatePersonCommand>()
            .With(x => x.PersonType, PersonType.Employee)
            .With(x => x.EmployeeRole, EmployeeRole.Detailer)
            .With(x => x.Document, faker.Person.Cpf())
            .With(x => x.Email, faker.Internet.Email())
            .Create();
        var updatedPerson = _fixture.Create<Person>();
        var personDto = _fixture.Create<PersonDto>();
        var phone = _fixture.Create<Phone>();
        var address = _fixture.Create<Address>();
        string password = PeopleFactory.CreateClient().Password;
        var person = _fixture.Create<Person>()
            .Update(input.Fullname, input.Document, input.PersonType, input.EmployeeRole, input.Email, password, phone, address);

        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<Phone>(input.Phone)).Returns(phone);
        _mapperMock.Setup(m => m.Map<Address>(input.Address)).Returns(address);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedPerson);
        _mapperMock.Setup(m => m.Map<PersonDto>(updatedPerson)).Returns(personDto);

        // Act
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(personDto);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var input = _fixture.Create<UpdatePersonCommand>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Person?) null);

        // Act
        var result = await _useCase.Handle(input, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
