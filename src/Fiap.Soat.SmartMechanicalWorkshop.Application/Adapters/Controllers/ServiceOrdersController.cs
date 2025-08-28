using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Mappers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class ServiceOrdersController(IMediator mediator) : IServiceOrdersController
{
    public async Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetServiceOrderByIdQuery(id), cancellationToken);
        var result = ResponseMapper.Map(response, ServiceOrderPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, Guid? personId, CancellationToken cancellationToken)
    {
        var query = new ListServiceOrdersQuery(paginatedRequest.PageNumber, paginatedRequest.PageSize, personId);
        var response = await mediator.Send(query, cancellationToken);
        var result = ResponseMapper.Map(response, ServiceOrderPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> GetAverageExecutionTime(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAverageExecutionTimeCommand(startDate, endDate), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken)
    {
        CreateServiceOrderCommand command = new(request.ClientId, request.VehicleId, request.ServiceIds, request.Title, request.Description);
        var response = await mediator.Send(command, cancellationToken);
        var result = ResponseMapper.Map(response, ServiceOrderPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteServiceOrderCommand(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> UpdateAsync(Guid id, UpdateOneServiceOrderRequest request, CancellationToken cancellationToken)
    {
        UpdateServiceOrderCommand command = new(id, request.Title, request.Description, request.ServiceIds);
        var response = await mediator.Send(command, cancellationToken);
        var result = ResponseMapper.Map(response, ServiceOrderPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> PatchAsync(Guid id, PatchServiceOrderRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateServiceOrderStatusCommand(id, request.Status), cancellationToken);
        var result = ResponseMapper.Map(response, ServiceOrderPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }
}
