using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Mappers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class SuppliesController(IMediator mediator) : ISuppliesController
{
    public async Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetSupplyByIdQuery(id), cancellationToken);
        var result = ResponseMapper.Map(response, SupplyPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> GetAllAsync(PaginatedRequest paginatedQuery, CancellationToken cancellationToken)
    {
        var response = await mediator.Send((ListSuppliesQuery) paginatedQuery, cancellationToken);
        var result = ResponseMapper.Map(response, SupplyPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> CreateAsync(CreateSupplyCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        var result = ResponseMapper.Map(response, SupplyPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteSupplyCommand(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> UpdateAsync(Guid id, UpdateOneSupplyRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateSupplyCommand(id, request.Name, request.Quantity, request.Price), cancellationToken);
        var result = ResponseMapper.Map(response, SupplyPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }
}
