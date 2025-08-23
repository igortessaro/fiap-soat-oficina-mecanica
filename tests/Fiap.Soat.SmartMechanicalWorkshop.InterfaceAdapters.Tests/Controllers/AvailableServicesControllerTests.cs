using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Tests.Controllers;

public sealed class AvailableServicesControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly AvailableServicesController _controller;

    public AvailableServicesControllerTests()
    {
        _controller = new AvailableServicesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedResult_WhenServiceIsCreated()
    {
        // Arrange
        var request = _fixture.Create<CreateAvailableServiceRequest>();
        var response = new Response<AvailableServiceDto>(_fixture.Create<AvailableServiceDto>(), HttpStatusCode.Created);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateAvailableServiceCommand>(), It.IsAny<CancellationToken>()))
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
    public async Task GetById_ShouldReturnOkResult_WhenServiceExists()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var query = new GetAvailableServiceByIdQuery(id);
        var response = new Response<AvailableServiceDto>(_fixture.Create<AvailableServiceDto>(), HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithListOfServices()
    {
        // Arrange
        var query = new ListAvailableServicesQuery(1, 10);
        var availableServices = _fixture.CreateMany<AvailableServiceDto>(10).ToList();
        var paginatedResponse = new Paginate<AvailableServiceDto>(availableServices, availableServices.Count, query.PageSize, query.PageNumber, 1);
        var response = new Response<Paginate<AvailableServiceDto>>(paginatedResponse, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListAvailableServicesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllAsync(query, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenServiceIsDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = ResponseFactory.Ok(HttpStatusCode.NoContent);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAvailableServiceCommand>(), It.IsAny<CancellationToken>()))
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
    public async Task Update_ShouldReturnOk_WhenServiceIsUpdated()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateOneAvailableServiceRequest>();
        var dto = _fixture.Create<AvailableServiceDto>();
        var response = new Response<AvailableServiceDto>(dto, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAvailableServiceCommand>(), It.IsAny<CancellationToken>()))
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
