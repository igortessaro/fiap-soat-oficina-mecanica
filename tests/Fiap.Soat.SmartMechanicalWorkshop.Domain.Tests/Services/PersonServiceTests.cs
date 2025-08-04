using AutoFixture;
using AutoMapper;
using Bogus;
using Bogus.Extensions.Brazil;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using FluentAssertions;
using Moq;
using System.Net;
using Person = Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities.Person;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class PersonServiceTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _service = new PersonService(_mapperMock.Object, _repositoryMock.Object, _addressRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedPerson()
    {
        // Arrange
        var request = _fixture.Build<CreatePersonRequest>()
            .With(x => x.PersonType, PersonType.Client)
            .Without(x => x.EmployeeRole)
            .Create();
        var person = PeopleFactory.CreateClient();
        var personDto = _fixture.Create<PersonDto>();

        _mapperMock.Setup(m => m.Map<Person>(request)).Returns(person);
        _repositoryMock.Setup(r => r.AddAsync(person, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonDto>(person)).Returns(personDto);

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(personDto);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenPersonExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var person = _fixture.Create<Person>();
        var address = _fixture.Create<Address>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _addressRepositoryMock.Setup(r => r.GetByIdAsync(person.AddressId, It.IsAny<CancellationToken>())).ReturnsAsync(address);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        _addressRepositoryMock.Verify(r => r.GetByIdAsync(person.AddressId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(person, It.IsAny<CancellationToken>()), Times.Once);
        _addressRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Address>(), It.IsAny<CancellationToken>()), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Person?) null);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
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
        var result = await _service.GetOneAsync(id, CancellationToken.None);

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
        var result = await _service.GetOneAsync(id, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedPerson_WhenExists()
    {
        // Arrange
        var faker = new Faker("pt_BR");
        var input = _fixture.Build<UpdateOnePersonInput>()
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
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(personDto);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var input = _fixture.Create<UpdateOnePersonInput>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Person?) null);

        // Act
        var result = await _service.UpdateAsync(input, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedPersons()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var paginate = _fixture.Create<Paginate<Person>>();
        var paginateDto = _fixture.Create<Paginate<PersonDto>>();

        _repositoryMock.Setup(r =>
                r.GetAllAsync(new List<string> { nameof(Person.Vehicles), nameof(Person.Address) }, paginatedRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<PersonDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _service.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }
}
