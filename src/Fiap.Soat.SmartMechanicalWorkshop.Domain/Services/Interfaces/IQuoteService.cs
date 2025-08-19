using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IQuoteService
{
    Task<Response<QuoteDto>> PatchAsync(Guid id, QuoteStatus status, CancellationToken cancellationToken);
}
