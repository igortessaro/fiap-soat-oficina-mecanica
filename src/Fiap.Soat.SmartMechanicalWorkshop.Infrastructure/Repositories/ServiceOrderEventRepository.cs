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
        var matchingIds = await Query()

            .GroupBy(e => e.ServiceOrderId)
            .Where(g =>
                g.Any(e => e.Status == EServiceOrderStatus.InProgress) &&
                g.Any(e => e.Status == EServiceOrderStatus.Delivered))
            .Select(g => g.Key)
            .ToListAsync(cancellationToken);

        var result = await Query()
             .Where(item => item.Status == EServiceOrderStatus.InProgress || item.Status == EServiceOrderStatus.Delivered)
            .Where(e => matchingIds.Contains(e.ServiceOrderId))
            .GroupBy(e => e.ServiceOrderId)
            .ToListAsync(cancellationToken);

        return result;
    }

}
