using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class ServiceOrderRepository(AppDbContext appDbContext) : Repository<ServiceOrder>(appDbContext), IServiceOrderRepository
{
    public async Task<ServiceOrder?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.ServiceOrderAvailableServices)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
