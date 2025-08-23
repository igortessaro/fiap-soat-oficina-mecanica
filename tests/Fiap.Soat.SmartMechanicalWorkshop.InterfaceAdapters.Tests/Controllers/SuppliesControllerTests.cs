using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Supplies;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Tests.Controllers;

public sealed class SuppliesControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly SuppliesController _controller;

    public SuppliesControllerTests()
    {
        _controller = new SuppliesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnOkResult_WhenSupplyExists()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = new Response<SupplyDto>(_fixture.Create<SupplyDto>(), HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.Is<GetSupplyByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetOneAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WithPaginatedSupplies()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var supplies = _fixture.CreateMany<SupplyDto>(5).ToList();
        var paginatedResponse = new Paginate<SupplyDto>(supplies, supplies.Count, paginatedRequest.PageSize, paginatedRequest.PageNumber, 1);
        var response = new Response<Paginate<SupplyDto>>(paginatedResponse, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListSuppliesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResult_WhenSupplyIsCreated()
    {
        // Arrange
        var command = _fixture.Create<CreateSupplyCommand>();
        var response = new Response<SupplyDto>(_fixture.Create<SupplyDto>(), HttpStatusCode.Created);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateAsync(command, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenSupplyIsDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = ResponseFactory.Ok(HttpStatusCode.NoContent);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSupplyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.DeleteAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenSupplyIsUpdated()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateOneSupplyRequest>();
        var dto = _fixture.Create<SupplyDto>();
        var response = new Response<SupplyDto>(dto, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSupplyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }
}
