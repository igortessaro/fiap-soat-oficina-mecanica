using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public class AvailableServiceRepository(AppDbContext appDbContext) : Repository<AvailableService>(appDbContext), IAvailableServiceRepository
{
    public async Task<AvailableService?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.Supplies)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AvailableService> UpdateAsync(Guid id, string name, decimal? price, IReadOnlyList<Supply> supplies, CancellationToken cancellationToken)
    {
        var entity = await Query(false)
            .Include(x => x.Supplies)
            .SingleAsync(x => x.Id == id, cancellationToken);

        _ = entity.Update(name, price);
        _ = entity.AddSupplies(supplies);
        _ = await UpdateAsync(entity, cancellationToken);
        return entity;
    }
}
