using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class QuoteRepository(AppDbContext appDbContext) : Repository<Quote>(appDbContext), IQuoteRepository
{
    public override Task<Quote?> GetDetailedByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Query().Where(x => x.Id == id)
            .Include(x => x.Supplies).ThenInclude(x => x.Supply)
            .Include(x => x.Services).ThenInclude(x => x.Service)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
