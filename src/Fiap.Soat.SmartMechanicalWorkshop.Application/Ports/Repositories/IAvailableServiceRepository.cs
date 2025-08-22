using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;

public interface IAvailableServiceRepository : IRepository<AvailableService>
{
    Task<AvailableService?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<AvailableService> UpdateAsync(Guid id, string name, decimal? price, IReadOnlyList<ServiceSupplyDto> supplies, CancellationToken cancellationToken);
}
