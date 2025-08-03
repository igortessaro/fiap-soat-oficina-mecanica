using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;

public interface IServiceOrderEventRepository : IRepository<ServiceOrderEvent>
{
    Task<ServiceOrderExecutionTimeReport> GetAverageExecutionTimesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
}
