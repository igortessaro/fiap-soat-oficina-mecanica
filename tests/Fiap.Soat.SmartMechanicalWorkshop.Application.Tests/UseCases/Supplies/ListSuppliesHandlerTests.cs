using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class ListSuppliesHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly ListSuppliesHandler _useCase;

    public ListSuppliesHandlerTests()
    {
        _useCase = new ListSuppliesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedSupplies()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListSuppliesQuery>();
        var paginate = _fixture.Create<Paginate<Supply>>();

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<PaginatedRequest>(), It.IsAny<CancellationToken>(), null)).ReturnsAsync(paginate);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginate);
    }
}
