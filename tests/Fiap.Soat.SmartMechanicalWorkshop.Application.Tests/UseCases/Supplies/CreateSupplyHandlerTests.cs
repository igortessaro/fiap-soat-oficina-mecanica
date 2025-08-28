using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.Supplies;

public sealed class CreateSupplyHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly CreateSupplyHandler _useCase;

    public CreateSupplyHandlerTests()
    {
        _useCase = new CreateSupplyHandler(_mapperMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedSupply()
    {
        // Arrange
        var request = _fixture.Create<CreateSupplyCommand>();
        var entity = _fixture.Create<Supply>();
        var createdEntity = _fixture.Create<Supply>();

        _mapperMock.Setup(m => m.Map<Supply>(request)).Returns(entity);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(createdEntity);

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(createdEntity);
    }
}
