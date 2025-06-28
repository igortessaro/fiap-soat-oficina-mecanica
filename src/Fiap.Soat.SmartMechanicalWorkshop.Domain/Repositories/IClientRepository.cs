using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<Client?> GetAsync(Guid id, CancellationToken cancellationToken);
}
