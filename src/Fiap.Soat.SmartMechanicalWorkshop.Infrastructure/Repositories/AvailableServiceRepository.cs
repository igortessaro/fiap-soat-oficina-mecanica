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
}
