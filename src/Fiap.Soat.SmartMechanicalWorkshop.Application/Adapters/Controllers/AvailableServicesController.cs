using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class AvailableServicesController(IMediator mediator) : IAvailableServicesController
{
    public async Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAvailableServiceByIdQuery(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<ActionResult> GetAllAsync(PaginatedRequest paginatedQuery, CancellationToken cancellationToken)
    {
        var response = await mediator.Send((ListAvailableServicesQuery) paginatedQuery, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken)
    {
        var supplies = request.Supplies?.Select(x => new CreateServiceSupplyCommand(x.SupplyId, x.Quantity)).ToList() ?? [];
        var command = new CreateAvailableServiceCommand(request.Name, request.Price, supplies);
        var response = await mediator.Send(command, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteAvailableServiceCommand(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> UpdateAsync(Guid id, UpdateOneAvailableServiceRequest request,
        CancellationToken cancellationToken)
    {
        var supplies = request.Supplies.Select(x => new UpdateServiceSupplyCommand(x.SupplyId, x.Quantity)).ToList();
        UpdateAvailableServiceCommand command = new(id, request.Name, request.Price, supplies);
        var response = await mediator.Send(command, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }
}
