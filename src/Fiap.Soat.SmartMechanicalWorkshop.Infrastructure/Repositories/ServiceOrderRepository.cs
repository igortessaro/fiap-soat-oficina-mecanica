using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class ServiceOrderRepository(AppDbContext appDbContext) : Repository<ServiceOrder>(appDbContext), IServiceOrderRepository
{
    public override async Task<ServiceOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await base.GetByIdAsync(id, cancellationToken);
        return result?.SyncState();
    }

    public async Task<ServiceOrder?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.AvailableServices)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ServiceOrder?> GetDetailedAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.AvailableServices).ThenInclude(item => item.Supplies)
            .Include(x => x.Client)
            .Include(x => x.Vehicle)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ServiceOrder?> GetOneByLoginAsync(GetOnePersonByLoginInput loginInput, CancellationToken cancellationToken)
    {
        return await Query()
            .Include(x => x.AvailableServices).ThenInclude(item => item.Supplies)
            .Include(x => x.Client)
            .Include(x => x.Vehicle)
            .FirstOrDefaultAsync(x => x.Id.Equals(loginInput.Id) && x.Client.Email.Address.Equals(loginInput.Email), cancellationToken);
    }
}
