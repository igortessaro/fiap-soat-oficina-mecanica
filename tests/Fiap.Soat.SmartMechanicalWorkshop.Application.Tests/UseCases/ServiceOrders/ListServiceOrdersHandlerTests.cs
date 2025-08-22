using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.ServiceOrders;

public sealed class ListServiceOrdersHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IServiceOrderRepository> _repositoryMock = new();
    private readonly ListServiceOrdersHandler _useCase;

    public ListServiceOrdersHandlerTests()
    {
        _useCase = new ListServiceOrdersHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginated()
    {
        // Arrange
        var paginate = _fixture.Create<Paginate<ServiceOrder>>();
        var paginateDto = _fixture.Create<Paginate<ServiceOrderDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<IReadOnlyList<string>>(), It.IsAny<PaginatedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<ServiceOrderDto>>(paginate)).Returns(paginateDto);

        // Act
        var result = await _useCase.Handle(new ListServiceOrdersQuery(10, 10, null), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }
}
