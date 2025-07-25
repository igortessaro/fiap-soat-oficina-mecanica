using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IAvailableServiceRepository : IRepository<AvailableService>
{
    Task<AvailableService?> GetAsync(Guid id, CancellationToken cancellationToken);
}
