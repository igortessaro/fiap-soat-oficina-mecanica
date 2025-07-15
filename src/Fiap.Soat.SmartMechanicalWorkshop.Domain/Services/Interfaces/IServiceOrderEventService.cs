using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IServiceOrderEventService
{
    Task<Response> CreateAsync(Guid serviceOrderId, EServiceOrderStatus status, CancellationToken cancellationToken);
}
