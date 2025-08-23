using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Presenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;

public sealed class VehiclesController(IMediator mediator) : IVehiclesController
{
    public async Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await mediator.Send((ListVehiclesQuery) paginatedRequest, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken)
    {
        CreateVehicleCommand command = new(request.LicensePlate, request.ManufactureYear, request.Brand, request.Model, request.PersonId);
        var response = await mediator.Send(command, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteVehicleCommand(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> UpdateAsync(Guid id, UpdateOneVehicleRequest request, CancellationToken cancellationToken)
    {
        UpdateVehicleCommand command = new(id, request.LicensePlate, request.ManufactureYear, request.Brand, request.Model, request.PersonId);
        var response = await mediator.Send(command, cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }
}
