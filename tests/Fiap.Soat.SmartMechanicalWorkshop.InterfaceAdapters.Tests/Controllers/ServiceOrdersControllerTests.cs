using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.ServiceOrders;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Tests.Controllers;

public sealed class ServiceOrdersControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly ServiceOrdersController _controller;

    public ServiceOrdersControllerTests()
    {
        _controller = new ServiceOrdersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnOkResult_WhenServiceOrderExists()
    {
        var id = _fixture.Create<Guid>();
        var response = new Response<ServiceOrderDto>(_fixture.Create<ServiceOrderDto>(), HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.Is<GetServiceOrderByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetOneAsync(id, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WithPaginatedServiceOrders()
    {
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var personId = _fixture.Create<Guid>();
        var serviceOrders = _fixture.CreateMany<ServiceOrderDto>(5).ToList();
        var paginatedResponse = new Paginate<ServiceOrderDto>(serviceOrders, serviceOrders.Count, paginatedRequest.PageSize, paginatedRequest.PageNumber, 1);
        var response = new Response<Paginate<ServiceOrderDto>>(paginatedResponse, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListServiceOrdersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetAllAsync(paginatedRequest, personId, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAverageExecutionTime_ShouldReturnOkResult()
    {
        var startDate = DateOnly.FromDateTime(_fixture.Create<DateTime>());
        var endDate = DateOnly.FromDateTime(_fixture.Create<DateTime>());
        var response = ResponseFactory.Ok<ServiceOrderExecutionTimeReportDto>(_fixture.Create<ServiceOrderExecutionTimeReportDto>());

        _mediatorMock.Setup(m => m.Send(It.Is<GetAverageExecutionTimeCommand>(c => c.StartDate == startDate && c.EndDate == endDate), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetAverageExecutionTime(startDate, endDate, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResult_WhenServiceOrderIsCreated()
    {
        var request = _fixture.Create<CreateServiceOrderRequest>();
        var dto = _fixture.Create<ServiceOrderDto>();
        var response = new Response<ServiceOrderDto>(dto, HttpStatusCode.Created);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateServiceOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        _mediatorMock.Setup(m => m.Publish(It.IsAny<UpdateServiceOrderStatusNotification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.CreateAsync(request, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenServiceOrderIsDeleted()
    {
        var id = _fixture.Create<Guid>();
        var response = ResponseFactory.Ok(HttpStatusCode.NoContent);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteServiceOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.DeleteAsync(id, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenServiceOrderIsUpdated()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateOneServiceOrderRequest>();
        var dto = _fixture.Create<ServiceOrderDto>();
        var response = new Response<ServiceOrderDto>(dto, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateServiceOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.UpdateAsync(id, request, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnOk_WhenServiceOrderStatusIsUpdated()
    {
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<PatchServiceOrderRequest>();
        var response = ResponseFactory.Ok<ServiceOrderDto>(_fixture.Create<ServiceOrderDto>());

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateServiceOrderStatusCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.PatchAsync(id, request, CancellationToken.None);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }
}
