using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public sealed class ServiceOrderRepository(AppDbContext appDbContext) : Repository<ServiceOrder>(appDbContext), IServiceOrderRepository
{

}
