using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.Controllers;

public sealed class VehiclesControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly VehiclesController _controller;

    public VehiclesControllerTests()
    {
        _controller = new VehiclesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnOkResult_WhenVehicleExists()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = new Response<VehicleDto>(_fixture.Create<VehicleDto>(), HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.Is<GetVehicleByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetOneAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WithPaginatedVehicles()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var vehicles = _fixture.CreateMany<VehicleDto>(5).ToList();
        var paginatedResponse = new Paginate<VehicleDto>(vehicles, vehicles.Count, paginatedRequest.PageSize, paginatedRequest.PageNumber, 1);
        var response = new Response<Paginate<VehicleDto>>(paginatedResponse, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListVehiclesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResult_WhenVehicleIsCreated()
    {
        // Arrange
        var request = _fixture.Create<CreateNewVehicleRequest>();
        var response = new Response<VehicleDto>(_fixture.Create<VehicleDto>(), HttpStatusCode.Created);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateVehicleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateAsync(request, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.Created);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenVehicleIsDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = ResponseFactory.Ok(HttpStatusCode.NoContent);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteVehicleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.DeleteAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenVehicleIsUpdated()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateOneVehicleRequest>();
        var dto = _fixture.Create<VehicleDto>();
        var response = new Response<VehicleDto>(dto, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateVehicleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }
}
