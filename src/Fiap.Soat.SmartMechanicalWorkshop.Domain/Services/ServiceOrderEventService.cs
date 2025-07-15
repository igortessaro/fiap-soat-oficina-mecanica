using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ServiceOrderEventService(IServiceOrderEventRepository repository) : IServiceOrderEventService
{
    public async Task<Response> CreateAsync(Guid serviceOrderId, EServiceOrderStatus status, CancellationToken cancellationToken)
    {
        _ = await repository.AddAsync(new ServiceOrderEvent(serviceOrderId, status), cancellationToken);
        return ResponseFactory.Ok();
    }
}
