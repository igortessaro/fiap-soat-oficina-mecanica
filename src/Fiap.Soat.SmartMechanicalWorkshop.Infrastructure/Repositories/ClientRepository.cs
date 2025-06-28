using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class ClientRepository(AppDbContext appDbContext) : Repository<Client>(appDbContext), IClientRepository
{
    public Task<Client?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        Query(false).Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
