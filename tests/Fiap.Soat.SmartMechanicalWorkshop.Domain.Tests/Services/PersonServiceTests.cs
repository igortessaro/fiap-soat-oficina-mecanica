using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class PersonServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _service = new PersonService(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedPerson()
    {
        // Arrange
        var request = _fixture.Build<CreatePersonRequest>()
            .With(x => x.PersonType, EPersonType.Client)
            .Without(x => x.EmployeeRole)
            .Create();
        var person = _fixture.Create<Person>();
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
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(person);

        // Act
        var result = await _service.DeleteAsync(id, CancellationToken.None);

        // Assert
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
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(person);
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
        var input = _fixture.Create<UpdateOnePersonInput>();
        var person = _fixture.Create<Person>();
        var updatedPerson = _fixture.Create<Person>();
        var personDto = _fixture.Create<PersonDto>();
        var phone = _fixture.Create<Phone>();
        var address = _fixture.Create<Address>();

        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<Phone>(input.Phone)).Returns(phone);
        _mapperMock.Setup(m => m.Map<Address>(input.Address)).Returns(address);
        person = person.Update(input.Fullname, input.Document, input.PersonType, input.EmployeeRole, input.Email, phone, address);
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

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<PersonDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _service.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }

    [Fact]
    public async Task GetOneByLoginAsync_ShouldReturnPerson_WhenExists()
    {
        // Arrange
        var loginRequest = _fixture.Create<LoginRequest>();
        var person = _fixture.Create<Person>();
        var personDto = _fixture.Create<PersonDto>();

        _repositoryMock.Setup(r => r.GetOneByLoginAsync(loginRequest, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonDto>(person)).Returns(personDto);

        // Act
        var result = await _service.GetOneByLoginAsync(loginRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(personDto);
    }

    [Fact]
    public async Task GetOneByLoginAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var loginRequest = _fixture.Create<LoginRequest>();
        _repositoryMock.Setup(r => r.GetOneByLoginAsync(loginRequest, It.IsAny<CancellationToken>())).ReturnsAsync((Person) null!);

        // Act
        var result = await _service.GetOneByLoginAsync(loginRequest, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
