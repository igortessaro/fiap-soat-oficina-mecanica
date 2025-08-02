using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IAvailableServiceRepository : IRepository<AvailableService>
{
    Task<AvailableService?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<AvailableService> UpdateAsync(Guid id, string name, decimal? price, IReadOnlyList<Supply> supplies, CancellationToken cancellationToken);
}
