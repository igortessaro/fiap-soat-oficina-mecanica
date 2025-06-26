using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public class SupplyRepository(AppDbContext appDbContext) : Repository<Supply>(appDbContext), ISupplyRepository
{
}