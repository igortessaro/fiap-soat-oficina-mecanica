using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class PersonRepository(AppDbContext appDbContext) : Repository<Person>(appDbContext), IPersonRepository
{
    public Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        Query(false)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public override Task<Person?> GetDetailedByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Query()
            .Include(x => x.Address)
            .Include(x => x.Vehicles)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        Query().FirstOrDefaultAsync(item => item.Email.Address.Equals(email), cancellationToken);
}
