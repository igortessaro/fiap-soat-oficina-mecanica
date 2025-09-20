using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;

public interface IQuoteController
{
    Task<IActionResult> PatchQuoteAsync(Guid id, Guid quoteId, QuoteStatus status, CancellationToken cancellationToken);
}
