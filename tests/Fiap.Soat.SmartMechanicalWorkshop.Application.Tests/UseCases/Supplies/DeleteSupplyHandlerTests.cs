using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class DeleteSupplyHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly DeleteSupplyHandler _useCase;

    public DeleteSupplyHandlerTests()
    {
        _useCase = new DeleteSupplyHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<Supply>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new DeleteSupplyCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _useCase.Handle(new DeleteSupplyCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
