using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<ServiceOrder?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<ServiceOrder?> GetDetailedAsync(Guid id, CancellationToken cancellationToken);
    Task<ServiceOrder> GetOneByLoginAsync(GetOnePersonByLoginInput getOnePersonByLoginInput, CancellationToken cancellationToken);
}
