using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class ListPeopleHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly ListPeopleHandler _useCase;

    public ListPeopleHandlerTests()
    {
        _useCase = new ListPeopleHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedPersons()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListPeopleQuery>();
        var paginate = _fixture.Create<Paginate<Person>>();
        var paginateDto = _fixture.Create<Paginate<PersonDto>>();

        _repositoryMock.Setup(r =>
                r.GetAllAsync(new List<string> { nameof(Person.Vehicles), nameof(Person.Address) }, paginatedRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<PersonDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }
}
