using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Person?> GetOneByLoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
}
