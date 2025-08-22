using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.People;

public sealed class DeletePersonHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IPersonRepository> _repositoryMock = new();
    private readonly DeletePersonHandler _useCase;

    public DeletePersonHandlerTests()
    {
        _useCase = new DeletePersonHandler(_repositoryMock.Object, _addressRepositoryMock.Object);
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
        var result = await _useCase.Handle(new DeletePersonCommand(id), CancellationToken.None);

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
        var result = await _useCase.Handle(new DeletePersonCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
