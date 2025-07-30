using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IServiceOrderEventRepository : IRepository<ServiceOrderEvent>
{
    Task<List<IGrouping<Guid, ServiceOrderEvent>>> GetServiceOrderEvents(CancellationToken cancellationToken);
}
