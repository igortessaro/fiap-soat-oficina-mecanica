using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class UpdateSupplyHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly UpdateSupplyHandler _useCase;

    public UpdateSupplyHandlerTests()
    {
        _useCase = new UpdateSupplyHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedSupply_WhenFound()
    {
        // Arrange
        var command = _fixture.Create<UpdateSupplyCommand>();
        var entity = _fixture.Create<Supply>();
        var updatedEntity = _fixture.Create<Supply>();

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Supply>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(updatedEntity);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        // Arrange
        var command = _fixture.Create<UpdateSupplyCommand>();
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Supply?) null);

        // Act
        var result = await _useCase.Handle(command, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }
}
