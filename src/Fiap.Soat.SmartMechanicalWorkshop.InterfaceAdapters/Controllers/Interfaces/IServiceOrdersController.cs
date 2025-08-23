using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.ServiceOrders;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface IServiceOrdersController
{
    Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, Guid? personId, CancellationToken cancellationToken);
    Task<IActionResult> GetAverageExecutionTime(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneServiceOrderRequest request, CancellationToken cancellationToken);
    Task<IActionResult> PatchAsync(Guid id, PatchServiceOrderRequest request, CancellationToken cancellationToken);
}
