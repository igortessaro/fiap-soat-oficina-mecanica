using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken);
}
