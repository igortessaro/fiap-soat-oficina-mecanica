using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.AvailableServices;

public sealed class DeleteAvailableServiceHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
    private readonly DeleteAvailableServiceHandler _useCase;

    public DeleteAvailableServiceHandlerTests()
    {
        _useCase = new DeleteAvailableServiceHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = _fixture.Create<AvailableService>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        // Act
        var result = await _useCase.Handle(new DeleteAvailableServiceCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        // Act
        var result = await _useCase.Handle(new DeleteAvailableServiceCommand(id), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
