using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<ServiceOrder?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<ServiceOrder?> GetDetailedAsync(Guid id, CancellationToken cancellationToken);
    Task<ServiceOrder> UpdateAsync(Guid id, string title, string description, IReadOnlyList<AvailableService> services, CancellationToken cancellationToken);
}
