using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
