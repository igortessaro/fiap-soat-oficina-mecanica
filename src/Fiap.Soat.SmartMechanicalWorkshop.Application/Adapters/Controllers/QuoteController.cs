using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class QuoteController(IMediator mediator) : IQuoteController
{
    public async Task<IActionResult> PatchQuoteAsync(Guid id, Guid quoteId, QuoteStatus status, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateQuoteStatusCommand(quoteId, status, id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }
}
