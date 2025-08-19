using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class ListSuppliesHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly ListSuppliesHandler _useCase;

    public ListSuppliesHandlerTests()
    {
        _useCase = new ListSuppliesHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedSupplies()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<ListSuppliesQuery>();
        var paginate = _fixture.Create<Paginate<Supply>>();
        var paginateDto = _fixture.Create<Paginate<SupplyDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<SupplyDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }
}
