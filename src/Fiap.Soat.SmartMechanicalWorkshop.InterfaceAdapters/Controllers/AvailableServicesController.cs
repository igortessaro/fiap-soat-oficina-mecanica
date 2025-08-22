using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Presenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;

public sealed class AvailableServicesController(IMediator mediator)
{
    public async Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAvailableServiceByIdQuery(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }
}
