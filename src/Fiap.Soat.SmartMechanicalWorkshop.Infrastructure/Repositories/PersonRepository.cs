using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class PersonRepository(AppDbContext appDbContext) : Repository<Person>(appDbContext), IPersonRepository
{
    public Task<Person?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        Query(false).Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
