using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public class ServiceOrderEventRepository(AppDbContext appDbContext) : Repository<ServiceOrderEvent>(appDbContext), IServiceOrderEventRepository
{
    public async Task<List<IGrouping<Guid, ServiceOrderEvent>>> GetServiceOrderEvents(CancellationToken cancellationToken)
    {
        var result = await Query()
            .Where(e => e.Status == EServiceOrderStatus.InProgress || e.Status == EServiceOrderStatus.Delivered)
            .GroupBy(e => e.ServiceOrderId)
            .Where(g =>
                g.Any(e => e.Status == EServiceOrderStatus.InProgress) &&
                g.Any(e => e.Status == EServiceOrderStatus.Delivered))
            .ToListAsync(cancellationToken);

        return result;
    }

}
