using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class ListPeopleHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly ListPeopleHandler _useCase;

    public ListPeopleHandlerTests()
    {
        _useCase = new ListPeopleHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedPersons()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListPeopleQuery>();
        var paginate = _fixture.Create<Paginate<Person>>();
        var paginateDto = _fixture.Create<Paginate<Person>>();

        _repositoryMock.Setup(r =>
                r.GetAllAsync(new List<string> { nameof(Person.Vehicles), nameof(Person.Address) }, It.IsAny<PaginatedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginate);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginate);
    }
}
